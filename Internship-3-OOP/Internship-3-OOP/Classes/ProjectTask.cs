using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internship_3_OOP.Classes
{
    public class ProjectTask
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public TaskStatus Status;
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
    }
}
