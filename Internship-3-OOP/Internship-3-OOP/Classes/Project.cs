using Internship_3_OOP.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internship_3_OOP.Classes
{
    public class Project
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartOfProject { get; private set; }
        public DateTime EndOfProject { get; private set; }
        public ProjectStatus Status;

        public Project(string projectName, string projectDescription, DateTime startOfProject, DateTime endOfProject)
        {
            Name = projectName;
            Description = projectDescription;
            StartOfProject = startOfProject;
            EndOfProject = endOfProject;
        }

        public void Active()
        {
            Status = ProjectStatus.Active;
        }

        public void OnHold()
        {
            Status = ProjectStatus.OnHold;
        }

        public void Finished()
        {
            Status = ProjectStatus.Finished;
        }
    }
}
