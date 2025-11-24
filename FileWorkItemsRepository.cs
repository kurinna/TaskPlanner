using Kurinna.TaskPlanner.DataAccess.Abstractions;
using Kurinna.TaskPlanner.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Kurinna.TaskPlanner.DataAccess
{
    public class FileWorkItemsRepository : IWorkItemsRepository
    {
        
        private const string WorkItemsFileName = "work-items.json";

        private readonly Dictionary<Guid, WorkItem> _workItems;

        public FileWorkItemsRepository()
        {
            _workItems = new Dictionary<Guid, WorkItem>();

            if (File.Exists(WorkItemsFileName))
            {
                try
                {
                    string jsonContent = File.ReadAllText(WorkItemsFileName);

                    if (!string.IsNullOrWhiteSpace(jsonContent))
                    {
                        WorkItem[] itemsArray = JsonConvert.DeserializeObject<WorkItem[]>(jsonContent);

                        if (itemsArray != null)
                        {
                            foreach (var item in itemsArray)
                            {
                                _workItems.Add(item.Id, item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading or deserializing {WorkItemsFileName}: {ex.Message}");
                }
            }
          
        }


        public Guid Add(WorkItem workItem)
        {
           
            WorkItem itemCopy = workItem.Clone();

            
            Guid newId = Guid.NewGuid();

            
            itemCopy.Id = newId;

            
            _workItems.Add(newId, itemCopy);

            return newId;
        }

        public WorkItem Get(Guid id)
        {
            if (_workItems.TryGetValue(id, out WorkItem workItem))
            {
                return workItem.Clone();
            }
            return null;
        }

        public WorkItem[] GetAll()
        {
            return _workItems.Values.Select(item => item.Clone()).ToArray();
        }

        public bool Update(WorkItem workItem)
        {
            if (_workItems.ContainsKey(workItem.Id))
            {
                _workItems[workItem.Id] = workItem.Clone();
                return true;
            }
            return false;
        }

        public bool Remove(Guid id)
        {
            return _workItems.Remove(id);
        }

        public void SaveChanges()
        {
            WorkItem[] itemsArray = _workItems.Values.ToArray();

            string jsonContent = JsonConvert.SerializeObject(itemsArray, Formatting.Indented);

            File.WriteAllText(WorkItemsFileName, jsonContent);
        }
    }
}
