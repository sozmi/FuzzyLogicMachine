using ClassLibraryFLM.Base;
using ClassLibraryFLM.Functions;
using ClassLibraryFLM.FuzzyLogic.Base;

namespace ClassLibraryFLM.FuzzyLogic.Production
{
    public class TestFuzzy
    {
        private static void TestStart(string name)
        {
            Console.WriteLine();
            Console.WriteLine("*********************************************************");
            Console.WriteLine($"Начало теста: {name}");
            var db = BaseRules.Obj();
            db.Clear();
        }

        private static void TestEnd(string name)
        {
            Console.WriteLine();
            Console.WriteLine($"Конец теста: {name}");
            Console.WriteLine("*********************************************************");
        }

        public static void TestSyntaxAnalyze()
        {
            TestStart("проверка синтаксического анализатора");
            //тест синтаксического анализатора на этапе аккумуляции
            var db = BaseRules.Obj();
            Variable test = db.AddLinguisticVariable("Test", new(0, 200));
            var low = test.AddTerm("10", FuncFabric.GetLine(1));
            var medium = test.AddTerm("100", FuncFabric.GetLine(1));
            var hight = test.AddTerm("1000", FuncFabric.GetLine(1));
            low.Value = 10;
            medium.Value = 100;
            hight.Value = 1000;
            Comment speedMedium = db.GetOrCreateFuzzyComment(new(test, medium));
            Comment speedLow = db.GetOrCreateFuzzyComment(new(test, low));
            Comment speedHight = db.GetOrCreateFuzzyComment(new(test, hight));

            List<Tuple<Condition, double>> conditions = [
                new(new($"{speedHight} ИЛИ ({speedMedium} И {speedLow})"),  1000),
            new(new($"{speedHight} И ({speedMedium} И {speedLow})"),    10),
            new(new($"{speedHight} И ({speedMedium} ИЛИ {speedLow})"),  100),
            new(new($"{speedHight} ИЛИ {speedMedium} И {speedLow}"),    10)
            ];
            for (int i = 0; i < conditions.Count; i++)
            {
                var c = conditions[i];
                Rule rule = new(c.Item1, new(new(""), new("", FuncFabric.GetFunc(ETypeFunc.None))));
                conditions[i] = new(db.AddRule(rule).Condition, c.Item2);
            }

            db.Aggregation();
            foreach (var c in conditions)
                CheckCondition(c);
            TestEnd("проверка синтаксического анализатора");
        }

        public static void TestFazzification()
        {
            TestStart("проверка этапа фаззификации");
            var db = BaseRules.Obj();
            //проверка фаззификации 
            Variable speed = db.AddLinguisticVariable("Скорость автомобиля", new(0, 200));
            var low = speed.AddTerm("низкая", FuncFabric.GetLineZ(30, 40));
            var medium = speed.AddTerm("средняя", FuncFabric.GetTriangle(35, 50, 65));
            var hight = speed.AddTerm("высокая", FuncFabric.GetLineS(60, 70));
            Comment speedMedium = new(speed, medium);
            Comment speedLow = new(speed, low);
            Comment speedOverMedium = new(speed, ETypeModificator.Very, medium);
            Comment speedHight = new(speed, hight);

            Condition conditionSpeed = new($"{speedHight} ИЛИ {speedMedium} И {speedLow} ИЛИ {speedOverMedium}");
            Rule rule = new(conditionSpeed, new(new(""), new("", new(ETypeFunc.None))));
            db.AddRule(rule);

            speed = db.GetLinguisticVariable("Скорость автомобиля") ?? throw new NullReferenceException("Для успокоение компилятора");
            db.SetValue(speed, 55);
            db.Fuzzy();
            TestEnd("проверка этапа фаззификации");
        }

        private static void CheckCondition(Tuple<Condition, double> tuple)
        {
            var condition = tuple.Item1;
            if (condition.Value == null)
            {
                Console.WriteLine($"{condition} = значение не установлено");
                return;
            }
            double value = Math.Round((double)condition.Value, 2);
            if (value >= tuple.Item2 - 0.001 && value <= tuple.Item2 + 0.001)
            {
                Console.WriteLine("Успех");
            }
            else
            {
                Console.WriteLine($"{condition} != {tuple.Item2}");
            }
        }
        public static void TestAggregation()
        {
            TestStart("проверка этапа агрегации");
            var db = BaseRules.Obj();
            Variable speed = db.AddLinguisticVariable("Скорость автомобиля", new(0, 200));
            speed.AddTerm("низкая", FuncFabric.GetLineZ(30, 40));
            var medium = speed.AddTerm("средняя", FuncFabric.GetTriangle(35, 50, 65));
            speed.AddTerm("высокая", FuncFabric.GetLineS(60, 70));

            Variable coffie = db.AddLinguisticVariable("Кофе", new(-40, 270));
            coffie.AddTerm("холодный", FuncFabric.GetLineZ(0, 23));
            coffie.AddTerm("комфортный", FuncFabric.GetP(20, 25, 30, 45));
            var hot = coffie.AddTerm("горячий", FuncFabric.GetLineS(30, 80));


            Comment coffieHot = new(coffie, hot);
            Comment speedMedium = new(speed, medium);


            List<Tuple<string, int>> vars = [new("Скорость автомобиля", 55), new("Кофе", 70)];
            foreach (var v in vars)
            {
                var it = db.GetLinguisticVariable(v.Item1) ?? throw new NullReferenceException("Для успокоение компилятора");
                db.SetValue(it, v.Item2);
            }

            List<Tuple<Condition, double>> conditions = [
                new(new($"{speedMedium} И {coffieHot}"),    0.67),
            new(new($"{speedMedium} ИЛИ {coffieHot}"),  0.8)
            ];
            for (int i = 0; i < conditions.Count; i++)
            {
                var c = conditions[i];
                Rule rule = new(c.Item1, new(new(""), new("", new(ETypeFunc.None))));
                conditions[i] = new(db.AddRule(rule).Condition, c.Item2);
            }

            db.Fuzzy();
            db.Aggregation();

            foreach (var c in conditions)
                CheckCondition(c);
            TestEnd("проверка этапа агрегации");
        }
        public static void TestActivation()
        {
            TestStart("проверка этапа активизации");
            var db = BaseRules.Obj();
            Variable speed = db.AddLinguisticVariable("Скорость автомобиля", new(0, 80));
            speed.AddTerm("низкая", FuncFabric.GetLineZ(30, 40));
            var medium = speed.AddTerm("средняя", FuncFabric.GetTriangle(35, 50, 65));
            speed.AddTerm("высокая", FuncFabric.GetLineS(60, 70));

            Variable coffie = db.AddLinguisticVariable("Кофе", new(0, 80));
            coffie.AddTerm("холодный", FuncFabric.GetLineZ(0, 23));
            coffie.AddTerm("комфортный", FuncFabric.GetP(20, 25, 30, 45));
            var hot = coffie.AddTerm("горячий", FuncFabric.GetLineS(30, 80));


            Comment coffieHot = new(coffie, hot);
            Comment speedMedium = new(speed, medium);

            List<Tuple<string, int>> vars = [new("Скорость автомобиля", 55)];
            foreach (var v in vars)
            {
                var it = db.GetLinguisticVariable(v.Item1) ?? throw new NullReferenceException("Для успокоение компилятора");
                db.SetValue(it, v.Item2);
            }


            List<Tuple<Condition, double>> conditions = [
                new(new($"{speedMedium}"), 0.67)
            ];
            for (int i = 0; i < conditions.Count; i++)
            {
                var c = conditions[i];
                Rule rule = new(c.Item1, coffieHot);
                conditions[i] = new(db.AddRule(rule).Condition, c.Item2);
            }

            db.Fuzzy();
            db.Aggregation();
            db.Activation();

            foreach (var c in conditions)
                CheckCondition(c);
            TestEnd("проверка этапа активации");
        }

        public static void TestAccumulation()
        {
            TestStart("проверка этапа аккумуляции");
            var db = BaseRules.Obj();

            Variable speed = db.AddLinguisticVariable("Скорость автомобиля", new(0, 200));
            var low = speed.AddTerm("низкая", AreaFuncs.GetFunc(ETypeAreaFunc.Intersection, [FuncFabric.GetLine(0.6), FuncFabric.GetLineZ(30, 40)]));
            var medium = speed.AddTerm("средняя", AreaFuncs.GetFunc(ETypeAreaFunc.Intersection, [FuncFabric.GetLine(0.7), FuncFabric.GetTriangle(35, 50, 65)]));
            var hight = speed.AddTerm("высокая", AreaFuncs.GetFunc(ETypeAreaFunc.Intersection, [FuncFabric.GetLine(0.5), FuncFabric.GetLineS(60, 70)]));
            Comment speedMedium = new(speed, medium);

            List<Tuple<string, int>> vars = [new("Скорость автомобиля", 55)];
            foreach (var v in vars)
            {
                var it = db.GetLinguisticVariable(v.Item1) ?? throw new NullReferenceException("Для успокоение компилятора");
                db.SetValue(it, v.Item2);
            }
            Rule rule = new(new(""), speedMedium)
            {
                OutVarFunction = AreaFuncs.GetFunc(ETypeAreaFunc.Union, [low.Function, medium.Function, hight.Function])
            };
            db.AddRule(rule);
            db.Acсumulation("Скорость автомобиля");
            Console.WriteLine(db);
            TestEnd("проверка этапа аккумуляции");
        }

        public static void TestUnFazzification()
        {
            TestStart("проверка этапа дефаззификации");
            var db = BaseRules.Obj();
            //проверка фаззификации 
            Variable speed = db.AddLinguisticVariable("Скорость автомобиля", new(0, 100));
            var low = speed.AddTerm("низкая", AreaFuncs.GetFunc(ETypeAreaFunc.Intersection, [FuncFabric.GetLine(0.8), FuncFabric.GetTriangle(10, 30, 50)]));
            var medium = speed.AddTerm("средняя", AreaFuncs.GetFunc(ETypeAreaFunc.Intersection, [FuncFabric.GetLine(0.6), FuncFabric.GetTriangle(35, 40, 50)]));
            var hight = speed.AddTerm("высокая", AreaFuncs.GetFunc(ETypeAreaFunc.Intersection, [FuncFabric.GetLine(0.7), FuncFabric.GetTriangle(35, 50, 65)]));
            var function = AreaFuncs.GetFunc(ETypeAreaFunc.Union, [low.Function, medium.Function, hight.Function]);
            Comment speedMedium = new(speed, medium);

            Rule rule = new(new(""), speedMedium);
            db.AddRule(rule);
            BaseRules.Unfuzzy(function, speed);

            TestEnd("проверка этапа фаззификации");
        }

        public static void TestCar() => Test("выбор машины", BaseRulesManager.TestCar, OutputManager.TestCar, false);
        public static void TestWater() => Test("воды", BaseRulesManager.TestWater, OutputManager.TestWater);

        public static void TestExam() => Test("подготовки к экзамену", BaseRulesManager.TestExam, OutputManager.TestExam);

        public static void TestWork() => Test("выбора работы", BaseRulesManager.TestWork, OutputManager.TestWork); 

        private static void Test(string name, Func<object> fillBase, Func<Pair<Variable, List<InputParam>>> fillInput, bool isMax = true)
        {
            TestStart(name);
            fillBase();
            var inputs = fillInput();
            Output.Calc(inputs.Item2, inputs.Item1, isMax);
            TestEnd(name);
        }


    }
}