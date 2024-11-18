using Internship_3_OOP.Enum;

namespace Internship_3_OOP.Classes
{
    public class ProjectTask
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public TasksStatus Status;
        public TaskPriority Priority;
        public int ExpectedDuration { get; set; }
        public Project ProjectName { get; set; }

        public ProjectTask(string taskName, string taskDescription, DateTime taskDeadline, int expectedDuration, Project projectName)
        {
            Name = taskName;
            Description = taskDescription;
            Deadline = taskDeadline;
            ExpectedDuration = expectedDuration;
            ProjectName = projectName;
        }

        public void Active()
        {
            Status = TasksStatus.Active;
        }

        public void Finished()
        {
            Status = TasksStatus.Finished;
        }

        public void Delayed()
        {
            Status = TasksStatus.Delayed;
        }

        public void HighPriority()
        {
            Priority = TaskPriority.High;
        }

        public void MediumPriority()
        {
            Priority = TaskPriority.Medium;
        }

        public void LowPriority()
        {
            Priority = TaskPriority.Low;
        }
    }
}
