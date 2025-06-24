using ClassLibraryFLM.Base;
using ClassLibraryFLM.FuzzyLogic.Base;
using ClassLibraryFLM.FuzzyLogic.Production;

namespace ClassLibraryFLM.FuzzyLogic.Test
{
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
            Variable difficultyLevel = BaseRules.Obj().GetLinguisticVariable("Сложность") ?? throw new NullReferenceException("Заполните базу правил");
            Variable preparationTime = BaseRules.Obj().GetLinguisticVariable("Время подготовки") ?? throw new NullReferenceException("Заполните базу правил");
            Variable cheatOpportunity = BaseRules.Obj().GetLinguisticVariable("Возможность списать") ?? throw new NullReferenceException("Заполните базу правил");
            Variable successChance = BaseRules.Obj().GetLinguisticVariable("Вероятность сдачи") ?? throw new NullReferenceException("Заполните базу правил");
            List<InputParam> inputs = [
                 new("Новые технологии в РПС", [new(difficultyLevel, 3), new(preparationTime, 5), new(cheatOpportunity, 8)]),
                 new("Интеллектальные информационные системы",          [new(difficultyLevel, 10), new(preparationTime, 2), new(cheatOpportunity, 4)]),
                 new("Моделирование",          [new(difficultyLevel, 6), new(preparationTime, 7), new(cheatOpportunity, 2)]),
                 new("Интеллектуальный анализ данных",       [new(difficultyLevel, 5), new(preparationTime, 3), new(cheatOpportunity, 4)]),
                 new("Английский язык",       [new(difficultyLevel, 1), new(preparationTime, 9), new(cheatOpportunity, 2)]),
            ];
            return new(successChance, inputs);
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
            Variable salary = BaseRules.Obj().GetLinguisticVariable("Зарплата") ?? throw new NullReferenceException("Заполните базу правил");
            Variable distance = BaseRules.Obj().GetLinguisticVariable("Работники на удалёнке") ?? throw new NullReferenceException("Заполните базу правил");
            Variable rating = BaseRules.Obj().GetLinguisticVariable("Рейтинг (отзывы)") ?? throw new NullReferenceException("Заполните базу правил");
            Variable descision = BaseRules.Obj().GetLinguisticVariable("Решение") ?? throw new NullReferenceException("Заполните базу правил");
            List<InputParam> inputs = [
                new("RusBitTech", [new(salary, 50), new(distance, 7), new(rating, 0.90)]),
                new("CPS",          [new(salary, 30), new(distance, 6), new(rating, 0.55)]),
                new("Axenix",          [new(salary, 50), new(distance, 15), new(rating, 0.75)]),
                new("NII",       [new(salary, 20), new(distance, 9), new(rating, 0.65)]),
                new("Private Company",       [new(salary, 60), new(distance, 11), new(rating, 0.45)])
                ];
            return new(descision, inputs);
        }
    }
}