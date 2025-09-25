using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kurinna.TaskPlanner.Domain.Models;
using Kurinna.TaskPlanner.Domain.Logic;


namespace Kurinna.TaskPlanner
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Workitem[] items =
                    {
            new Workitem("Write report", new DateTime(2025, 9, 20), Priority.High),
            new Workitem("Fix bugs", new DateTime(2025, 9, 15), Priority.Medium),
            new Workitem("Team meeting", new DateTime(2025, 9, 12), Priority.High),
            new Workitem("Prepare slides", new DateTime(2025, 9, 18), Priority.Low),
            new Workitem("Code review", new DateTime(2025, 9, 11), Priority.Medium),
            new Workitem("Client call", new DateTime(2025, 9, 10), Priority.High),
            new Workitem("Update docs", new DateTime(2025, 9, 17), Priority.Low),
            new Workitem("Deploy app", new DateTime(2025, 9, 13), Priority.High),
            new Workitem("Design UI", new DateTime(2025, 9, 16), Priority.Medium),
            new Workitem("Backup DB", new DateTime(2025, 9, 14), Priority.Low)
        };
            SimpleTaskPlanner planner = new SimpleTaskPlanner();

            bool running = true;
            while (running)
            {
                // Сортуємо та виводимо
                var sortedItems = planner.CreatePlan(items.ToArray());

                Console.WriteLine("\n--- Поточний список завдань ---");
                foreach (var item in sortedItems)
                {
                    Console.WriteLine($"{item.priority,-6} {item.DueDate.ToShortDateString(),-12} {item.Title}");
                }

                Console.Write("\nБажаєте додати новий елемент масиву? (y/n): ");
                string answer = Console.ReadLine()?.Trim().ToLower();

                if (answer == "n")
                {
                    running = false;
                    Console.WriteLine("Програма завершена.");
                }
                else if (answer == "y")
                {
                    // Зчитуємо новий елемент
                    Console.Write("Введіть назву завдання: ");
                    string title = Console.ReadLine();

                    // Рік
                    int year;
                    while (true)
                    {
                        Console.Write("Введіть рік: ");
                        if (int.TryParse(Console.ReadLine(), out year) && year > 0) break;
                        Console.WriteLine("Некоректний рік. Спробуйте ще раз.");
                    }

                    // Місяць
                    int month;
                    while (true)
                    {
                        Console.Write("Введіть місяць (1-12): ");
                        if (int.TryParse(Console.ReadLine(), out month) && month >= 1 && month <= 12) break;
                        Console.WriteLine("Некоректний місяць. Спробуйте ще раз.");
                    }

                    // День
                    int day;
                    while (true)
                    {
                        Console.Write("Введіть число: ");
                        if (int.TryParse(Console.ReadLine(), out day))
                        {
                            // перевіряємо чи існує дата
                            try
                            {
                                var testDate = new DateTime(year, month, day);
                                break;
                            }
                            catch
                            {
                                Console.WriteLine("Такої дати не існує. Спробуйте ще раз.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Некоректне число. Спробуйте ще раз.");
                        }
                    }

                    // Пріоритет
                    Priority priority;
                    while (true)
                    {
                        Console.WriteLine("Оберіть пріоритет:");
                        foreach (var pr in Enum.GetValues(typeof(Priority)))
                        {
                            Console.WriteLine($"- {pr}");
                        }
                        Console.Write("Введіть пріоритет: ");
                        string priorityInput = Console.ReadLine();

                        if (Enum.TryParse(priorityInput, true, out priority)) break;
                        Console.WriteLine("Некоректний пріоритет. Спробуйте ще раз.");
                    }

                    items = items.Append(new Workitem(title, new DateTime(year, month, day), priority)).ToArray();

                    Console.WriteLine("\nНовий елемент додано.\n");
                }
                else
                {
                    Console.WriteLine("Невірна відповідь. Введіть 'y' або 'n'.");
                }
            }
        }
    }
}
