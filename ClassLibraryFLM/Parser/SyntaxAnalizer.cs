using ClassLibraryFLM.FuzzyLogic;
using ClassLibraryFLM.FuzzyLogic.Base;
using ClassLibraryFLM.FuzzyLogic.Production;

namespace ClassLibraryFLM.Parser
{
    // <выражение> = '(' <выражение> ')' | <высказывание> |  <выражение> <бин. оператор> <выражение>
    // <бин. оператор> := 'И' | 'ИЛИ'
    // <модификатор> := ОЧЕНЬ | БОЛЕЕ ИЛИ МЕНЕЕ | МНОГО БОЛЬШЕ | НЕ

    // <высказывание> :=  <наименование> 'есть' <терм> | <наименование> 'есть' <модификатор> <терм>
    // <наименование> := '[' 'строка' ']' 
    // <терм> := '[' 'строка' ']' 

    public class SyntaxAnalizer(string input)
    {
        readonly Lexer lex = new(input);
        readonly BaseRules db = BaseRules.Obj();

        public string Input => lex.input;

        bool NextLexemCheck(TokenType token, bool showError = true)
        {
            lex.Next();
            return CheckLexem(token, showError);
        }
        bool CheckLexem(TokenType token, bool showError = true)
        {
            if (lex.CurrentToken?.Type != token)
            {
                if (showError)
                    Console.WriteLine($"Ожидалось {token}, а была получена {lex.CurrentToken?.Type}");
                return false;
            }
            return true;
        }
        Comment ParseComment(Variable pLingVar)
        {
            lex.Next();
            ETypeModificator modificator = ETypeModificator.None;
            if (CheckLexem(TokenType.Modifier, false))
            {
                modificator = ETypeModificator.Very;
                lex.Next();
            }

            if (CheckLexem(TokenType.Identificator))
            {
                var pTerm = pLingVar.GetTerm(ParseName()) ??
                    throw new NullReferenceException($"Не получили значение терма\"{ParseName()}\"");
                return db.GetOrCreateFuzzyComment(new(pLingVar, modificator, pTerm));

            }

            throw new Exception("Это не высказывание");
        }

        string ParseName()
        {
            if (lex.CurrentToken is Token cur)
            {
                return cur.Value.Trim('[', ']');
            }
            throw new NullReferenceException("Хрень");
        }

        double ParseBinaryOperator()
        {
            double a = 0;
            if (!CheckLexem(TokenType.And, false) || !CheckLexem(TokenType.Or, false) && CheckLexem(TokenType.Identificator))
            {
                var pLingVar = db.GetLinguisticVariable(ParseName()) ??
                    throw new NullReferenceException($"Не получили значения лингвистической переменной \"{ParseName()}\"");
                lex.Next();
                if (CheckLexem(TokenType.Has, false)) // проверка это высказывание?
                {
                    var pComment = ParseComment(pLingVar) ??
                        throw new NullReferenceException("Не получилось разобрать высказывание");
                    a = pComment.Function();
                    lex.Next();
                }

            }
            while (CheckLexem(TokenType.And, false) || CheckLexem(TokenType.Or, false))
            {
                Token oper = lex.CurrentToken ?? throw new NullReferenceException("Оператор не оператор"); ;
                lex.Next();
                var b = ParseExpression();

                if (oper.Type == TokenType.And)
                {
                    a = Math.Min(a, b);
                    lex.Next();
                }
                else if (oper.Type == TokenType.Or)
                {
                    a = Math.Max(a, b);
                    lex.Next();
                }
            }
            return a;
        }

        double ParseExpression()
        {
            // <подвыражение> = '(' <выражение> ')' | <высказывание> |  <подвыражение> <бин. оператор> <подвыражение>
            if (CheckLexem(TokenType.Left_Bracket, false))
            {
                lex.Next();
                var result = ParseBinaryOperator();
                CheckLexem(TokenType.Right_Bracket);
                return result;
            }

            if (CheckLexem(TokenType.Identificator))
            {
                if (lex.CurrentToken is Token)
                {
                    var pLingVar = db.GetLinguisticVariable(ParseName()) ??
                        throw new NullReferenceException($"Не получили значения лингвистической переменной \"{ParseName()}\"");
                    lex.Next();
                    if (CheckLexem(TokenType.Has, false)) // проверка это высказывание?
                    {
                        var pComment = ParseComment(pLingVar);
                        return pComment.Function();
                    }
                }
            }
            throw new Exception("Магия");
        }


        public void ReInit()
        {
            lex.Reinit();
        }
        public double Eval()
        {
            ReInit();
            lex.Next();

            return ParseBinaryOperator();
        }

        internal List<Comment> GetInputVars()
        {
            ReInit();
            List<Comment> comments = [];
            lex.Next();
            while (!CheckLexem(TokenType.EOF, false))
            {
                while (!CheckLexem(TokenType.Identificator, false))
                {
                    lex.Next();
                    if (CheckLexem(TokenType.EOF, false))
                        return comments;
                }


                if (lex.CurrentToken is Token leftToken)
                {
                    if (!NextLexemCheck(TokenType.Has, false))
                        continue;
                    var pLingVar = db.GetLinguisticVariable(leftToken.Value.Trim('[', ']')) ??
                                     throw new NullReferenceException("Не найдена лингвистическая переменная");
                    var pComment = ParseComment(pLingVar);
                    comments.Add(pComment);
                }
            }
            return comments;
        }
    }
}