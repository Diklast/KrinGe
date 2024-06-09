using System;
using System.Collections.Generic;

namespace BankApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            // Пример данных
            var банки = new List<Банк>
            {
                new Банк
                {
                    Название = "Банк А",
                    Филиалы = new List<Филиал>
                    {
                        new Филиал
                        {
                            Название = "Филиал 1",
                            Вклады = new List<Вклад>
                            {
                                new ДолгосрочныйВклад { ФИО = "Иван Иванов", Сумма = 1000 },
                                new ВкладДоВостребования { ФИО = "Мария Иванова", Сумма = 500 }
                            }
                        },
                        new Филиал
                        {
                            Название = "Филиал 2",
                            Вклады = new List<Вклад>
                            {
                                new ДолгосрочныйВклад { ФИО = "Петр Петров", Сумма = 2000 },
                                new ВкладДоВостребования { ФИО = "Анна Петрова", Сумма = 1500 }
                            }
                        }
                    }
                }
            };

            while (true)
            {
                Console.WriteLine("Хотите выполнить поиск? (y/n)");
                if (Console.ReadLine().ToLower() != "y")
                {
                    break;
                }

                Console.WriteLine("Введите название класса (Банк, Филиал, Вклад, ДолгосрочныйВклад, ВкладДоВостребования):");
                string className = Console.ReadLine();

                Console.WriteLine("Введите атрибут для поиска (например, Название, ФИО, Сумма):");
                string attribute = Console.ReadLine();

                Console.WriteLine("Введите значение для поиска:");
                string value = Console.ReadLine();

                var results = Поиск(банки, className, attribute, value);
                ПоказатьРезультаты(results);
            }
        }

        static IEnumerable<object> Поиск(List<Банк> банки, string className, string attribute, string value)
        {
            var результаты = new List<object>();

            foreach (var банк in банки)
            {
                if (className == nameof(Банк) && ПолучитьЗначениеСвойства(банк, attribute)?.ToString().Contains(value) == true)
                {
                    результаты.Add(банк);
                }

                foreach (var филиал in банк.Филиалы)
                {
                    if (className == nameof(Филиал) && ПолучитьЗначениеСвойства(филиал, attribute)?.ToString().Contains(value) == true)
                    {
                        результаты.Add(филиал);
                    }

                    foreach (var вклад in филиал.Вклады)
                    {
                        if (className == nameof(Вклад) && ПолучитьЗначениеСвойства(вклад, attribute)?.ToString().Contains(value) == true)
                        {
                            результаты.Add(вклад);
                        }

                        if (className == nameof(ДолгосрочныйВклад) && вклад is ДолгосрочныйВклад долгосрочныйВклад && ПолучитьЗначениеСвойства(долгосрочныйВклад, attribute)?.ToString().Contains(value) == true)
                        {
                            результаты.Add(долгосрочныйВклад);
                        }

                        if (className == nameof(ВкладДоВостребования) && вклад is ВкладДоВостребования вкладДоВостребования && ПолучитьЗначениеСвойства(вкладДоВостребования, attribute)?.ToString().Contains(value) == true)
                        {
                            результаты.Add(вкладДоВостребования);
                        }
                    }
                }
            }

            return результаты;
        }

        static object ПолучитьЗначениеСвойства(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);
        }

        static void ПоказатьРезультаты(IEnumerable<object> результаты)
        {
            foreach (var result in результаты)
            {
                Console.WriteLine(result);
            }
        }
    }

    class Банк
    {
        public string Название { get; set; }
        public List<Филиал> Филиалы { get; set; }
        public override string ToString() => $"Банк: {Название}, Филиалов: {Филиалы.Count}";
    }

    class Филиал
    {
        public string Название { get; set; }
        public List<Вклад> Вклады { get; set; }
        public override string ToString() => $"Филиал: {Название}, Вкладов: {Вклады.Count}";
    }

    abstract class Вклад
    {
        public string ФИО { get; set; }
        public decimal Сумма { get; set; }
        public abstract decimal РассчитатьСумму(int месяцы);
        public override string ToString() => $"{GetType().Name}: {ФИО}, Сумма: {Сумма}";
    }

    class ДолгосрочныйВклад : Вклад
    {
        public override decimal РассчитатьСумму(int месяцы)
        {
            return Сумма * (1 + 0.05m * месяцы / 12);
        }
    }

    class ВкладДоВостребования : Вклад
    {
        public override decimal РассчитатьСумму(int месяцы)
        {
            return Сумма;
        }
    }
}
