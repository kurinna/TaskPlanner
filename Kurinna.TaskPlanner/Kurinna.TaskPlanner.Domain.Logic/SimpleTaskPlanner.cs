using System;
using Kurinna.TaskPlanner.Domain.Models;
using System.Linq;

namespace Kurinna.TaskPlanner.Domain.Logic
{
    public class SimpleTaskPlanner
    {
        public Workitem[] CreatePlan(Workitem[] items)
        {
            var itemsAsList = items.ToList();
            itemsAsList.Sort(CompareWorkItems);
            return itemsAsList.ToArray();
        }
        private static int CompareWorkItems(Workitem firstItem, Workitem secondItem)
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
