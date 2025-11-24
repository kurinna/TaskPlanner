using System;
using System.Collections.Generic;
using System.Linq;
using Kurinna.TaskPlanner.Domain.Models;
using Kurinna.TaskPlanner.Domain.Logic;
using Kurinna.TaskPlanner.DataAccess;
using Kurinna.TaskPlanner.DataAccess.Abstractions;

namespace Kurinna.TaskPlanner
{
    internal static class Program
    {
        private static Guid ReadGuid(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (Guid.TryParse(Console.ReadLine(), out Guid id))
                {
                    return id;
                }
                Console.WriteLine("Некоректний формат GUID. Спробуйте ще раз.");
            }
        }

        private static void ShowMenu()
        {
            Console.WriteLine("\n--- Меню ---");
            Console.WriteLine("1 - Додати завдання");
            Console.WriteLine("2 - Показати план");
            Console.WriteLine("3 - Позначити завдання як виконане");
            Console.WriteLine("4 - Видалити завдання");
            Console.WriteLine("5 - Завершити роботу");
            Console.Write("Ваш вибір: ");
        }

        private static void AddWorkItem(IWorkItemsRepository repository)
        {
            Console.WriteLine("\n--- Додавання нового завдання ---");

            Console.Write("Введіть назву завдання: ");
            string title = Console.ReadLine();

            DateTime dueDate;
            while (true)
            {
                Console.Write("Введіть дату виконання (рррр-мм-дд): ");
                if (DateTime.TryParse(Console.ReadLine(), out dueDate)) break;
                Console.WriteLine("Некоректний формат дати. Спробуйте ще раз.");
            }

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

            WorkItem newItem = new WorkItem(title, dueDate, priority);

            repository.Add(newItem);
            repository.SaveChanges();

            Console.WriteLine("\nНовий елемент додано та збережено.\n");
        }

        private static void BuildPlan(SimpleTaskPlanner planner, IWorkItemsRepository repository)
        {
            WorkItem[] sortedItems = planner.CreatePlan();

            Console.WriteLine("\n--- План виконання завдань (відсортований) ---");
            if (sortedItems.Any())
            {
                Console.WriteLine($"{"ID",-36} | {"Пріоритет",-10} | {"Дата",-12} | {"Завершено",-10} | {"Назва"}");
                Console.WriteLine(new string('-', 90));

                foreach (var item in sortedItems)
                {
                    string completedStatus = item.IsCompleted ? "Так" : "Ні";
                    Console.WriteLine($"{item.Id,-36} | {item.priority,-10} | {item.DueDate.ToShortDateString(),-12} | {completedStatus,-10} | {item.Title}");
                }
            }
            else
            {
                Console.WriteLine("Немає завдань для відображення.");
            }
        }

        private static void MarkCompleted(IWorkItemsRepository repository)
        {
            Console.WriteLine("\n--- Позначити завдання як виконане ---");
            Guid id = ReadGuid("Введіть ID завдання, яке потрібно позначити як виконане: ");

            WorkItem itemToUpdate = repository.Get(id);

            if (itemToUpdate == null)
            {
                Console.WriteLine($"Завдання з ID {id} не знайдено.");
                return;
            }

            if (itemToUpdate.IsCompleted)
            {
                Console.WriteLine("Це завдання вже позначено як виконане.");
                return;
            }

            itemToUpdate.IsCompleted = true;

            if (repository.Update(itemToUpdate))
            {
                repository.SaveChanges();
                Console.WriteLine($"Завдання '{itemToUpdate.Title}' успішно позначено як виконане та збережено.");
            }
            else
            {
                Console.WriteLine("Помилка при оновленні завдання.");
            }
        }

        private static void RemoveWorkItem(IWorkItemsRepository repository)
        {
            Console.WriteLine("\n--- Видалити завдання ---");
            Guid id = ReadGuid("Введіть ID завдання, яке потрібно видалити: ");

            if (repository.Remove(id))
            {
                repository.SaveChanges();
                Console.WriteLine($"Завдання з ID {id} успішно видалено та збережено.");
            }
            else
            {
                Console.WriteLine($"Завдання з ID {id} не знайдено.");
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            IWorkItemsRepository repository = new FileWorkItemsRepository();

            SimpleTaskPlanner planner = new SimpleTaskPlanner(repository);

            bool running = true;
            while (running)
            {
                ShowMenu();
                string choice = Console.ReadLine()?.Trim().ToUpper();

                switch (choice)
                {
                    case "1":
                        AddWorkItem(repository);
                        break;

                    case "2":
                        BuildPlan(planner, repository);
                        break;

                    case "3":
                        MarkCompleted(repository);
                        break;

                    case "4":
                        RemoveWorkItem(repository);
                        break;

                    case "5":
                        running = false;
                        Console.WriteLine("Збереження змін та завершення програми...");
                        repository.SaveChanges();
                        break;

                    default:
                        Console.WriteLine("Невідома команда. Введіть 1, 2, 3, 4 або 5.");
                        break;
                }
            }

            Console.WriteLine("Програма завершена.");
        }
    }
}