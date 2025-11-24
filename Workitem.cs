using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurinna.TaskPlanner.Domain.Models
{
    public class WorkItem
    {
        public Guid Id { get; set; } 

        public DateTime CreationDate;
        public DateTime DueDate;
        public Priority priority;
        public Complexity complexity;
        public string Title;
        public string Description;
        public bool IsCompleted;

        public override string ToString()
        {
            return Title + ": due" + DueDate + ", " + priority.ToString().ToLower();
        }

        public WorkItem(string title, DateTime date, Priority p)
        {
            Id = Guid.Empty; 
            Title = title;
            DueDate = date;
            priority = p;
            IsCompleted = false;
            Description = string.Empty;
            CreationDate = DateTime.Now;
        }

        public WorkItem Clone()
        {

            return new WorkItem(Title, DueDate, priority)
            {
                Id = this.Id,
                CreationDate = this.CreationDate,
                complexity = this.complexity,
                Description = this.Description,
                IsCompleted = this.IsCompleted
            };

        }
    }
}
