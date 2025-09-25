using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurinna.TaskPlanner.Domain.Models
{
    public class Workitem
    {
        public DateTime CreationDate;
        public DateTime DueDate;
        public Priority priority;
        public Complexity complexity;
        public string Title;
        public string Description;
        public bool IsCompleted;
        public string ToString()
        {
            return Title + ": due" + DueDate + ", " + priority.ToString().ToLower();
        }
        public Workitem(string title, DateTime date, Priority p) {
            Title = title;
            DueDate = date;
            priority = p;
            IsCompleted = false;
            Description = string.Empty;
            CreationDate = DateTime.Now;

        }
    }
}
