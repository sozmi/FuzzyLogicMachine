//using ClassLibraryNL.FuzzyLogic;

// <выражение> = <ун. оператор> <подвыражение> | <подвыражение>
// <подвыражение> = '(' <выражение> ')' | <высказывание> |  <подвыражение> <бин. оператор> <подвыражение>

// <ун. оператор> := 'НЕ'
// <бин. оператор> := 'И' | 'ИЛИ'
// <модификатор> := ОЧЕНЬ | БОЛЕЕ ИЛИ МЕНЕЕ | МНОГО БОЛЬШЕ

// <высказывание> :=  <наименование> 'есть' <терм> | <наименование> 'есть' <модификатор> <терм>
// <наименование> := '[' 'строка' ']' 
// <терм> := '[' 'строка' ']' 


// Типы токенов
// Класс для лексического парсера
class Lexer
{
    public readonly string input;
    private int position;
    public Token? CurrentToken { get; private set; }

    private readonly List<Token> keywords;

    public Lexer(string input)
    {
        this.input = input.Trim();
        Reinit();
        keywords = [
            new(TokenType.Left_Bracket, "("),
            new(TokenType.Has, "есть"),
            new(TokenType.Has, "-"),
            new(TokenType.Right_Bracket,")"),
            new(TokenType.No, "НЕ"),
            new(TokenType.Or, "ИЛИ"),
            new(TokenType.And, "И"),
            new(TokenType.No, "не"),
            new(TokenType.Or, "или"),
            new(TokenType.And, "и"),
            new(TokenType.Modifier, "ОЧЕНЬ"),
            new(TokenType.Modifier, "БОЛЕЕ ИЛИ МЕНЕЕ"),
            new(TokenType.Modifier, "МНОГО БОЛЬШЕ")
            ];
        keywords = [.. keywords.OrderBy(x => x.Value.Length).Reverse()];
    }

    public void Reinit()
    {
        this.position = 0;
        this.CurrentToken = null;
    }
    public Token? Next()
    {
        if (position < input.Length)
        {
            char currentChar = input[position];

            while (char.IsWhiteSpace(currentChar))
            {
                // пропускаем белые символы
                currentChar = input[++position];
            }

            var keyword = GetKeyword();
            if (currentChar == '[')
            {
                // парсим идентификатор в квадратных скобках
                CurrentToken = ParseIdentifier();
                return CurrentToken;
            }
            else if (keyword is Token k)
            {
                CurrentToken = k;
                return CurrentToken;
            }
            else
            {
                throw new Exception($"Непредвиденный символ: {currentChar}");
            }
        }

        // добавляем маркер конца ввода
        CurrentToken = new Token(TokenType.EOF, "");
        return CurrentToken;
    }
    private Token? GetKeyword()
    {
        foreach (Token k in keywords)
            if (TryReadKeyword(k.Value))
                return k;
        return null;
    }

    // Парсим идентификатор в квадратных скобках
    private Token ParseIdentifier()
    {
        int start = position;
        position++; // пропускаем открывающую скобку '['
        while (position < input.Length && input[position] != ']')
            position++;
        if (position >= input.Length)
            throw new Exception("Ожидалась закрывающая скобка \"]\" для идентификатора.");
        string identifier = input.Substring(start + 1, position - start - 1);
        position++; // пропускаем закрывающую скобку ']'
        return new Token(TokenType.Identificator, "[" + identifier + "]");
    }

    // Попытка прочитать указанное ключевое слово
    private bool TryReadKeyword(string keyword)
    {
        if (input.Length - position >= keyword.Length &&
            input.Substring(position, keyword.Length) == keyword)
        {
            position += keyword.Length;
            return true;
        }
        return false;
    }
}
