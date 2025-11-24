using System;
using Kurinna.TaskPlanner.Domain.Models;
using System.Linq;
using Kurinna.TaskPlanner.DataAccess.Abstractions;

namespace Kurinna.TaskPlanner.Domain.Logic
{
    public class SimpleTaskPlanner
    {
        private readonly IWorkItemsRepository _repository;

        public SimpleTaskPlanner(IWorkItemsRepository repository)
        {
            _repository = repository;
        }

        public WorkItem[] CreatePlan()
        {
            WorkItem[] allItems = _repository.GetAll();

            var relevantItems = allItems
                .Where(item => !item.IsCompleted)
                .ToList();

            relevantItems.Sort(CompareWorkItems);

            return relevantItems.ToArray();
        }

        private static int CompareWorkItems(WorkItem firstItem, WorkItem secondItem)
        {
            int priorityComparison = secondItem.priority.CompareTo(firstItem.priority);
            if (priorityComparison != 0)
                return priorityComparison;

            int dueDateComparison = firstItem.DueDate.CompareTo(secondItem.DueDate);
            if (dueDateComparison != 0)
                return dueDateComparison;

            return string.Compare(firstItem.Title, secondItem.Title, StringComparison.OrdinalIgnoreCase);
        }
    }
}