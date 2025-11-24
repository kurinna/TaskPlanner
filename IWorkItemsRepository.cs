using System;
using Kurinna.TaskPlanner.Domain.Models;

namespace Kurinna.TaskPlanner.DataAccess.Abstractions
{
    public interface IWorkItemsRepository
    {
        
        Guid Add(WorkItem workItem);

        WorkItem Get(Guid id);

        WorkItem[] GetAll();

        bool Update(WorkItem workItem);

        bool Remove(Guid id);

        void SaveChanges();
    }
}