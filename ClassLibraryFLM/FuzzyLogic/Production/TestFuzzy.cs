using ClassLibraryFLM.Base;
using ClassLibraryFLM.Functions;
using ClassLibraryFLM.FuzzyLogic.Base;

namespace ClassLibraryFLM.FuzzyLogic.Production
{
    public interface ITestList<T>
    {
        public abstract static T TestWater();
        public abstract static T TestCar();
        public abstract static T TestExam();
        public abstract static T TestWork();
    }
    /// <summary>
    /// Класс должен заполнить базу знаний
    /// </summary>
    public class BaseRulesManager : ITestList<object>
    {
        public static object TestCar()
        {
            var db = BaseRules.Obj();
            #region Создание переменных
            Variable cost = new("Стоимость") // в т.р
            {
                Universe = new(0, 500),
                Terms = [
                    new("низкая", FuncFabric.GetLineZ(150, 250)),
                new("приемлимая", FuncFabric.GetTriangle(150,250,350)),
                new("макс.возможная", FuncFabric.GetTriangle(250,350,450)),
                new("высокая", FuncFabric.GetLineS(350, 450))
                    ]
            };
            Variable expenses = new("Эксплуатационные расходы") // в %
            {
                Universe = new(0, 30),
                Terms = [
                    new("низкие", FuncFabric.GetLineZ(5, 10)),
                new("оптимальные", FuncFabric.GetTrapetial(5,10,15,20)),
                new("неприемлемые", FuncFabric.GetLineS(15, 20)),
                ]
            };

            Variable reliability = new("Надежность")
            {
                Universe = new(0, 1),
                Terms = [
                    new("низкая", FuncFabric.GetLineZ(0.25, 0.5)),
                new("средняя", FuncFabric.GetTriangle(0.25, 0.5, 0.75)),
                new("высокая", FuncFabric.GetTriangle(0.4, 0.6, 0.8)),
                new("безупречная", FuncFabric.GetLineS(0.7, 0.9))
                    ]
            };

            Variable solution = new("Решение")
            {
                Universe = new(0, 1),
                Terms = [
                    new("да", FuncFabric.GetLineZ(0, 0.3)),
                new("скорее да, чем нет", FuncFabric.GetTriangle(0, 0.3, 0.6)),
                new("скорее нет, чем да", FuncFabric.GetTriangle(0.4, 0.7, 1)),
                new("нет", FuncFabric.GetLineS(0.7, 1))
                    ]
            };

            db.AddLinguisticVariable(cost);
            db.AddLinguisticVariable(expenses);
            db.AddLinguisticVariable(reliability);
            db.AddLinguisticVariable(solution);
            #endregion

            #region Задание правил
            //1) Если стоимость - низкая и эксплуатационные расходы - низкие и надежность -низкая, то решение - скорее да, чем нет.
            db.AddRule(new("[Стоимость] - [низкая] и [Эксплуатационные расходы] - [низкие] и [Надежность] - [низкая]", "[Решение] - [скорее да, чем нет]"));
            //2) Если стоимость - низкая и эксплуатационные расходы - низкие и надежность -средняя, то решение - скорее да, чем нет.
            db.AddRule(new("[Стоимость] - [низкая] и [Эксплуатационные расходы] - [низкие] и [Надежность] - [средняя]", "[Решение] - [скорее да, чем нет]"));
            //3) Если стоимость - низкая и эксплуатационные расходы - низкие и надежность -высокая, то решение - скорее да, чем нет.
            db.AddRule(new("[Стоимость] - [низкая] и [Эксплуатационные расходы] - [низкие] и [Надежность] - [высокая]", "[Решение] - [скорее да, чем нет]"));
            //4) Если стоимость - низкая и эксплуатационные расходы - низкие и надежность - безупречная, то решение - да.
            db.AddRule(new("[Стоимость] - [низкая] и [Эксплуатационные расходы] - [низкие] и [Надежность] - [безупречная]", "[Решение] - [да]"));
            //5) Если стоимость - низкая и эксплуатационные расходы - оптимальные и надежность - низкая, то решение -скорее нет, чем да.
            db.AddRule(new("[Стоимость] - [низкая] и [Эксплуатационные расходы] - [оптимальные] и [Надежность] - [низкая]", "[Решение] - [скорее нет, чем да]"));
            //6) Если стоимость - низкая и эксплуатационные расходы - оптимальные и надежность - средняя, то решение -скорее да, чем нет.
            db.AddRule(new("[Стоимость] - [низкая] и [Эксплуатационные расходы] - [оптимальные] и [Надежность] - [средняя]", "[Решение] - [скорее да, чем нет]"));
            //7) Если стоимость - низкая и эксплуатационные расходы - оптимальные и надежность - высокая, то решение -скорее да, чем нет.
            db.AddRule(new("[Стоимость] - [низкая] и [Эксплуатационные расходы] - [оптимальные] и [Надежность] - [высокая]", "[Решение] - [скорее да, чем нет]"));
            //8) Если стоимость - низкая и эксплуатационные расходы - оптимальные и надежность - безупречная, то решение -да.
            db.AddRule(new("[Стоимость] - [низкая] и [Эксплуатационные расходы] - [оптимальные] и [Надежность] - [безупречная]", "[Решение] - [да]"));
            //9) Если стоимость - низкая и эксплуатационные расходы - неприемлемые и надежность - низкая, то решение -нет.
            db.AddRule(new("[Стоимость] - [низкая] и [Эксплуатационные расходы] - [неприемлемые] и [Надежность] - [низкая]", "[Решение] - [нет]"));
            //10) Если стоимость - низкая и эксплуатационные расходы - неприемлемые и надежность - средняя, то решение -нет.
            db.AddRule(new("[Стоимость] - [низкая] и [Эксплуатационные расходы] - [неприемлемые] и [Надежность] - [средняя]", "[Решение] - [нет]"));
            //11) Если стоимость - низкая и эксплуатационные расходы - неприемлемые и надежность - высокая, то решение -нет.
            db.AddRule(new("[Стоимость] - [низкая] и [Эксплуатационные расходы] - [неприемлемые] и [Надежность] - [высокая]", "[Решение] - [нет]"));
            //12) Если стоимость - низкая и эксплуатационные расходы - неприемлемые и надежность - безупречная, то решение -нет.
            db.AddRule(new("[Стоимость] - [низкая] и [Эксплуатационные расходы] - [неприемлемые] и [Надежность] - [безупречная]", "[Решение] - [нет]"));
            //13) Если стоимость - приемлемая и эксплуатационные расходы - низкие и надежность - низкая, то решение -скорее да, чем нет.
            db.AddRule(new("[Стоимость] - [приемлимая] и [Эксплуатационные расходы] - [низкие] и [Надежность] - [низкая]", "[Решение] - [скорее да, чем нет]"));
            //14) Если стоимость - приемлемая и эксплуатационные расходы - низкие и надежность - средняя, то решение -скорее да, чем нет.
            db.AddRule(new("[Стоимость] - [приемлимая] и [Эксплуатационные расходы] - [низкие] и [Надежность] - [средняя]", "[Решение] - [скорее да, чем нет]"));
            //15) Если стоимость - приемлемая и эксплуатационные расходы - низкие и надежность - высокая, то решение -скорее да, чем нет.
            db.AddRule(new("[Стоимость] - [приемлимая] и [Эксплуатационные расходы] - [низкие] и [Надежность] - [высокая]", "[Решение] - [скорее да, чем нет]"));
            //16) Если стоимость - приемлемая и эксплуатационные расходы - низкие и надежность - безупречная, то решение -да.
            db.AddRule(new("[Стоимость] - [приемлимая] и [Эксплуатационные расходы] - [низкие] и [Надежность] - [безупречная]", "[Решение] - [да]"));
            //17) Если стоимость - приемлемая и эксплуатационные расходы - оптимальные и надежность - низкая, то решение -скорее нет, чем да.
            db.AddRule(new("[Стоимость] - [приемлимая] и [Эксплуатационные расходы] - [оптимальные] и [Надежность] - [низкая]", "[Решение] - [скорее нет, чем да]"));
            //18) Если стоимость -приемлемая и эксплуатационные расходы - оптимальные и надежность - средняя, то решение -скорее да, чем нет.
            db.AddRule(new("[Стоимость] - [приемлимая] и [Эксплуатационные расходы] - [оптимальные] и [Надежность] - [средняя]", "[Решение] - [скорее да, чем нет]"));
            //19) Если стоимость - приемлемая и эксплуатационные расходы - оптимальные и надежность - высокая, то решение -скорее да, чем нет.
            db.AddRule(new("[Стоимость] - [приемлимая] и [Эксплуатационные расходы] - [оптимальные] и [Надежность] - [высокая]", "[Решение] - [скорее да, чем нет]"));
            //20) Если стоимость - приемлемая и эксплуатационные расходы - оптимальные и надежность - безупречная, то решение -скорее да, чем нет.
            db.AddRule(new("[Стоимость] - [приемлимая] и [Эксплуатационные расходы] - [оптимальные] и [Надежность] - [безупречная]", "[Решение] - [скорее да, чем нет]"));
            //21) Если стоимость - приемлемая и эксплуатационные расходы - неприемлемые и надежность - низкая, то решение -нет.
            db.AddRule(new("[Стоимость] - [приемлимая] и [Эксплуатационные расходы] - [неприемлемые] и [Надежность] - [низкая]", "[Решение] - [нет]"));
            //22) Если стоимость - приемлемая и эксплуатационные расходы - неприемлемые и надежность - средняя, то решение -нет.
            db.AddRule(new("[Стоимость] - [приемлимая] и [Эксплуатационные расходы] - [неприемлемые] и [Надежность] - [средняя]", "[Решение] - [нет]"));
            //23) Если стоимость - приемлемая и эксплуатационные расходы - неприемлемые и надежность - высокая, то решение -нет.
            db.AddRule(new("[Стоимость] - [приемлимая] и [Эксплуатационные расходы] - [неприемлемые] и [Надежность] - [высокая]", "[Решение] - [нет]"));
            //24) Если стоимость - приемлемая и эксплуатационные расходы - неприемлемые и надежность - безупречная, то решение -нет.
            db.AddRule(new("[Стоимость] - [приемлимая] и [Эксплуатационные расходы] - [неприемлемые] и [Надежность] - [безупречная]", "[Решение] - [нет]"));
            //25) Если стоимость - макс.возможная и эксплуатационные расходы - низкие и надежность - низкая, то решение -нет.
            db.AddRule(new("[Стоимость] - [макс.возможная] и [Эксплуатационные расходы] - [низкие] и [Надежность] - [низкая]", "[Решение] - [нет]"));
            //26) Если стоимость - макс.возможная и эксплуатационные расходы - низкие и надежность - средняя, то решение -нет.
            db.AddRule(new("[Стоимость] - [макс.возможная] и [Эксплуатационные расходы] - [низкие] и [Надежность] - [средняя]", "[Решение] - [нет]"));
            //27) Если стоимость - макс.возможная и эксплуатационные расходы - низкие и надежность - высокая, то решение -скорее да, чем нет.
            db.AddRule(new("[Стоимость] - [макс.возможная] и [Эксплуатационные расходы] - [низкие] и [Надежность] - [высокая]", "[Решение] - [скорее да, чем нет]"));
            //28) Если стоимость - макс.возможная и эксплуатационные расходы - низкие и надежность - безупречная, то решение -скорее да, чем нет.
            db.AddRule(new("[Стоимость] - [макс.возможная] и [Эксплуатационные расходы] - [низкие] и [Надежность] - [безупречная]", "[Решение] - [скорее да, чем нет]"));
            //29) Если стоимость - макс.возможная и эксплуатационные расходы - оптимальные и надежность - низкая, то решение -нет.
            db.AddRule(new("[Стоимость] - [макс.возможная] и [Эксплуатационные расходы] - [оптимальные] и [Надежность] - [низкая]", "[Решение] - [нет]"));
            //30) Если стоимость - макс.возможная и эксплуатационные расходы - оптимальные и надежность - средняя, то решение -нет.
            db.AddRule(new("[Стоимость] - [макс.возможная] и [Эксплуатационные расходы] - [оптимальные] и [Надежность] - [средняя]", "[Решение] - [нет]"));
            //31) Если стоимость - макс.возможная и эксплуатационные расходы - оптимальные и надежность - высокая, то решение -скорее нет, чем да.
            db.AddRule(new("[Стоимость] - [макс.возможная] и [Эксплуатационные расходы] - [оптимальные] и [Надежность] - [высокая]", "[Решение] - [скорее нет, чем да]"));
            //32) Если стоимость - макс.возможная и эксплуатационные расходы - оптимальные и надежность - безупречная, то решение -скорее нет, чем да.
            db.AddRule(new("[Стоимость] - [макс.возможная] и [Эксплуатационные расходы] - [оптимальные] и [Надежность] - [безупречная]", "[Решение] - [скорее нет, чем да]"));
            //33) Если стоимость - макс.возможная и эксплуатационные расходы - неприемлемые и надежность - низкая, то решение -нет.
            db.AddRule(new("[Стоимость] - [макс.возможная] и [Эксплуатационные расходы] - [неприемлемые] и [Надежность] - [низкая]", "[Решение] - [нет]"));
            //34) Если стоимость - макс.возможная и эксплуатационные расходы - неприемлемые и надежность - средняя, то решение -нет.
            db.AddRule(new("[Стоимость] - [макс.возможная] и [Эксплуатационные расходы] - [неприемлемые] и [Надежность] - [средняя]", "[Решение] - [нет]"));
            //35) Если стоимость - макс.возможная и эксплуатационные расходы - неприемлемые и надежность - высокая, то решение -нет.
            db.AddRule(new("[Стоимость] - [макс.возможная] и [Эксплуатационные расходы] - [неприемлемые] и [Надежность] - [высокая]", "[Решение] - [нет]"));
            //36) Если стоимость - макс.возможная и эксплуатационные расходы - неприемлемые и надежность - безупречная, то решение -нет.
            db.AddRule(new("[Стоимость] - [макс.возможная] и [Эксплуатационные расходы] - [неприемлемые] и [Надежность] - [безупречная]", "[Решение] - [нет]"));
            //37) Если стоимость - высокая и эксплуатационные расходы - низкие и надежность -низкая, то решение - нет.
            db.AddRule(new("[Стоимость] - [высокая] и [Эксплуатационные расходы] - [низкие] и [Надежность] - [низкая]", "[Решение] - [нет]"));
            //38) Если стоимость - высокая и эксплуатационные расходы - низкие и надежность -средняя, то решение -нет.
            db.AddRule(new("[Стоимость] - [высокая] и [Эксплуатационные расходы] - [низкие] и [Надежность] - [средняя]", "[Решение] - [нет]"));
            //39) Если стоимость - высокая и эксплуатационные расходы - низкие и надежность -высокая, то решение -нет.
            db.AddRule(new("[Стоимость] - [высокая] и [Эксплуатационные расходы] - [низкие] и [Надежность] - [высокая]", "[Решение] - [нет]"));
            //40) Если стоимость - высокая и эксплуатационные расходы - низкие и надежность - безупречная, то решение - нет.
            db.AddRule(new("[Стоимость] - [высокая] и [Эксплуатационные расходы] - [низкие] и [Надежность] - [безупречная]", "[Решение] - [нет]"));
            //41) Если стоимость - высокая и эксплуатационные расходы - оптимальные и надежность - низкая, то решение - нет.
            db.AddRule(new("[Стоимость] - [высокая] и [Эксплуатационные расходы] - [оптимальные] и [Надежность] - [низкая]", "[Решение] - [нет]"));
            //42) Если стоимость - высокая и эксплуатационные расходы - оптимальные и надежность - средняя, то решение - нет.
            db.AddRule(new("[Стоимость] - [высокая] и [Эксплуатационные расходы] - [оптимальные] и [Надежность] - [средняя]", "[Решение] - [нет]"));
            //43) Если стоимость - высокая и эксплуатационные расходы - оптимальные и надежность - высокая, то решение - нет.
            db.AddRule(new("[Стоимость] - [высокая] и [Эксплуатационные расходы] - [оптимальные] и [Надежность] - [высокая]", "[Решение] - [нет]"));
            //44) Если стоимость - высокая и эксплуатационные расходы - оптимальные и надежность - безупречная, то решение - нет.
            db.AddRule(new("[Стоимость] - [высокая] и [Эксплуатационные расходы] - [оптимальные] и [Надежность] - [безупречная]", "[Решение] - [нет]"));
            //45) Если стоимость - высокая и эксплуатационные расходы - неприемлемые и надежность - низкая, то решение -нет.
            db.AddRule(new("[Стоимость] - [высокая] и [Эксплуатационные расходы] - [неприемлемые] и [Надежность] - [низкая]", "[Решение] - [нет]"));
            //46) Если стоимость - высокая и эксплуатационные расходы - неприемлемые и надежность - средняя, то решение -нет.
            db.AddRule(new("[Стоимость] - [высокая] и [Эксплуатационные расходы] - [неприемлемые] и [Надежность] - [средняя]", "[Решение] - [нет]"));
            //47) Если стоимость -высокая и эксплуатационные расходы - неприемлемые и надежность - высокая, то решение -нет.
            db.AddRule(new("[Стоимость] - [высокая] и [Эксплуатационные расходы] - [неприемлемые] и [Надежность] - [высокая]", "[Решение] - [нет]"));
            //48) Если стоимость -высокая и эксплуатационные расходы - неприемлемые и надежность - безупречная, то решение -нет.
            db.AddRule(new("[Стоимость] - [высокая] и [Эксплуатационные расходы] - [неприемлемые] и [Надежность] - [безупречная]", "[Решение] - [нет]"));
            #endregion
            return new();
        }

        public static object TestExam()
        {
            throw new NotImplementedException();
        }

        public static object TestWater()
        {
            var db = BaseRules.Obj();
            #region Создание переменных
            Variable liquidLevel = new("Уровень жидкости", new(0, 10))
            {
                Terms = [
                    new("малый", FuncFabric.GetLineZ(2,4)),
                new("средний", FuncFabric.GetTrapetial(2,4,6,8)),
                new("большой", FuncFabric.GetLineS(6,8)),
                ]
            };

            Variable liquidConsumption = new("Расход жидкости", new(0, 0.5))
            {
                Terms = [
                    new("малый", FuncFabric.GetLineZ(0.2, 0.3)),
                new("средний", FuncFabric.GetTrapetial(0.15, 0.25, 0.35, 0.45)),
                new("большой", FuncFabric.GetLineS(0.3, 0.4)),
                ]
            };

            Variable liquidFlow = new("Приток жидкости", new(0, 0.5))
            {
                Terms = [
                    new("малый", FuncFabric.GetLineZ(0.2, 0.3)),
                new("средний", FuncFabric.GetTrapetial(0.15, 0.25, 0.35, 0.45)),
                new("большой", FuncFabric.GetLineS(0.3, 0.4)),
                ]
            };

            db.AddLinguisticVariable(liquidLevel);
            db.AddLinguisticVariable(liquidConsumption);
            db.AddLinguisticVariable(liquidFlow);
            #endregion

            #region Создание правил

            List<Rule> fuzzyRules = [
                new("[Уровень жидкости] - [малый] И [Расход жидкости] - [большой]",     "[Приток жидкости] - [большой]"),
            new("[Уровень жидкости] - [малый]   И [Расход жидкости] - [средний]",   "[Приток жидкости] - [большой]"),
            new("[Уровень жидкости] - [малый]   И [Расход жидкости] - [малый]  ",   "[Приток жидкости] - [средний]"),
            new("[Уровень жидкости] - [средний] И [Расход жидкости] - [большой]",   "[Приток жидкости] - [большой]"),
            new("[Уровень жидкости] - [средний] И [Расход жидкости] - [средний]",   "[Приток жидкости] - [средний]"),
            new("[Уровень жидкости] - [средний] И [Расход жидкости] - [малый]  ",   "[Приток жидкости] - [средний]"),
            new("[Уровень жидкости] - [большой] И [Расход жидкости] - [большой]",   "[Приток жидкости] - [средний]"),
            new("[Уровень жидкости] - [большой] И [Расход жидкости] - [средний]",   "[Приток жидкости] - [малый]"),
            new("[Уровень жидкости] - [большой] И [Расход жидкости] - [малый]  ",   "[Приток жидкости] - [малый]")
            ];
            foreach (var rule in fuzzyRules)
                db.AddRule(rule);
            #endregion
            return new();
        }

        public static object TestWork()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Формирует массив входных переменных
    /// </summary>
    public class OutputManager : ITestList<Pair<Variable, List<InputParam>>>
    {
        public static Pair<Variable, List<InputParam>> TestCar()
        {
            Variable cost = BaseRules.Obj().GetLinguisticVariable("Стоимость") ?? throw new NullReferenceException("Заполните базу правил");
            Variable expenses = BaseRules.Obj().GetLinguisticVariable("Эксплуатационные расходы") ?? throw new NullReferenceException("Заполните базу правил");
            Variable reliability = BaseRules.Obj().GetLinguisticVariable("Надежность") ?? throw new NullReferenceException("Заполните базу правил");
            Variable solution = BaseRules.Obj().GetLinguisticVariable("Решение") ?? throw new NullReferenceException("Заполните базу правил");
            List<InputParam> inputs = [
            new("Toyota Yaris", [new(cost, 350), new(expenses, 5), new(reliability, 0.90)]),
            new("ВАЗ",          [new(cost, 350), new(expenses, 10), new(reliability, 0.50)]),
            new("BMW",          [new(cost, 500), new(expenses, 7), new(reliability, 0.85)]),
            new("Nissan",       [new(cost, 350), new(expenses, 7), new(reliability, 0.80)]),
            new("Hundaw",       [new(cost, 350), new(expenses, 7), new(reliability, 0.75)]),
            new("ОКА",          [new(cost, 200), new(expenses, 15), new(reliability, 0.45)])
    ];
            return new(solution, inputs);
        }

        public static Pair<Variable, List<InputParam>> TestExam()
        {
            throw new NotImplementedException();
        }

        public static Pair<Variable, List<InputParam>> TestWater()
        {
            var liquidLevel = BaseRules.Obj().GetLinguisticVariable("Уровень жидкости") ?? throw new NullReferenceException("Заполните базу правил");
            var liquidConsumption = BaseRules.Obj().GetLinguisticVariable("Расход жидкости") ?? throw new NullReferenceException("Заполните базу правил");
            var liquidFlow = BaseRules.Obj().GetLinguisticVariable("Приток жидкости") ?? throw new NullReferenceException("Заполните базу правил");
            List<InputParam> inputs = [
                new("Первый способ", [new(liquidLevel, 2.5), new(liquidConsumption, 0.4)]),
            ];
            return new(liquidFlow, inputs);
        }

        public static Pair<Variable, List<InputParam>> TestWork()
        {
            throw new NotImplementedException();
        }
    }
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