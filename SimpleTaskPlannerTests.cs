using System;
using System.Linq;
using Xunit;
using Moq;
using Kurinna.TaskPlanner.Domain.Models;
using Kurinna.TaskPlanner.Domain.Logic;
using Kurinna.TaskPlanner.DataAccess.Abstractions;

namespace Kurinna.TaskPlanner.Domain.Logic.Tests
{
    public class SimpleTaskPlannerTests
    {
        private readonly DateTime _baseDate = new DateTime(2025, 10, 20);

        private WorkItem[] GetTestWorkItems()
        {
            return new WorkItem[]
            {
                new WorkItem("Task B (Medium, Due Soon)", _baseDate.AddDays(1), Priority.Medium) { Id = Guid.NewGuid() },

                new WorkItem("Task D (Low, Due Later)", _baseDate.AddDays(10), Priority.Low) { Id = Guid.NewGuid() },

                new WorkItem("Completed Task", _baseDate.AddDays(1), Priority.High) { Id = Guid.NewGuid(), IsCompleted = true },

                new WorkItem("Task A (High, Due Today)", _baseDate, Priority.High) { Id = Guid.NewGuid() },

                new WorkItem("Task C (High, Due Today)", _baseDate, Priority.High) { Id = Guid.NewGuid() },
            };
        }


        [Fact]
        public void CreatePlan_ShouldSortItemsByPriorityAndDueDateAndExcludeCompleted()
        {

            var mockRepository = new Mock<IWorkItemsRepository>();
            var testItems = GetTestWorkItems();

            mockRepository.Setup(repo => repo.GetAll()).Returns(testItems);

            var planner = new SimpleTaskPlanner(mockRepository.Object);

            var plan = planner.CreatePlan();

            Assert.Equal(4, plan.Length);

            Assert.DoesNotContain(plan, item => item.IsCompleted);

            Assert.Equal("Task A (High, Due Today)", plan[0].Title);

            Assert.Equal("Task C (High, Due Today)", plan[1].Title);

            Assert.Equal("Task B (Medium, Due Soon)", plan[2].Title);

            Assert.Equal("Task D (Low, Due Later)", plan[3].Title);
        }

        [Fact]
        public void CreatePlan_ShouldReturnEmptyArrayWhenNoItems()
        {
            var mockRepository = new Mock<IWorkItemsRepository>();
            mockRepository.Setup(repo => repo.GetAll()).Returns(new WorkItem[0]);

            var planner = new SimpleTaskPlanner(mockRepository.Object);

            var plan = planner.CreatePlan();

            Assert.NotNull(plan);
            Assert.Empty(plan);
        }

        [Fact]
        public void CreatePlan_ShouldExcludeAllItemsIfAllAreCompleted()
        {
            var mockRepository = new Mock<IWorkItemsRepository>();
            var allCompletedItems = new WorkItem[]
            {
                new WorkItem("C1", _baseDate.AddDays(1), Priority.High) { IsCompleted = true },
                new WorkItem("C2", _baseDate.AddDays(2), Priority.Low) { IsCompleted = true }
            };

            mockRepository.Setup(repo => repo.GetAll()).Returns(allCompletedItems);

            var planner = new SimpleTaskPlanner(mockRepository.Object);

            var plan = planner.CreatePlan();

            Assert.Empty(plan);
        }
    }
}