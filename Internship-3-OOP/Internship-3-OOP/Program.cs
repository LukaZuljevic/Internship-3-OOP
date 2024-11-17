using System;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using Internship_3_OOP.Classes;
using Internship_3_OOP.Enum;

namespace Internship_3_OOP
{
    internal class Program
    {
        static Dictionary<Project, List<ProjectTask>> allProjects = new Dictionary<Project, List<ProjectTask>>();

        static void Main(string[] args)
        {
            var testProject1 = new Project("Projekt A", "Prvi test projekt", new DateTime(2024, 11, 1), new DateTime(2024, 12, 31));
            var testProject2 = new Project("Projekt B", "Drugi test projekt", new DateTime(2024, 10, 1), new DateTime(2024, 11, 30));
            var testProject3 = new Project("Projekt C", "Treci test projekt", new DateTime(2024, 11, 10), new DateTime(2025, 1, 31));

            var testTask1 = new ProjectTask("Dizajn pocetne stranice", "Izrada dizajna", new DateTime(2024, 11, 20), 10000, testProject1);
            var testTask2 = new ProjectTask("Implementacija featurea", "Novi feature", new DateTime(2024, 12, 15), 12000, testProject1);
            var testTask3 = new ProjectTask("Projektna specifikacija", "Izrada detaljne specifikacije za projekt", new DateTime(2024, 11, 23), 8000, testProject2);
            var testTask4 = new ProjectTask("Planiranje projekta", "Postavljanje planova i ciljeva", new DateTime(2024, 11, 15), 7000, testProject3);
            var testTask5 = new ProjectTask("Kodiranje modula", "Implementacija osnovnih funkcionalnosti", new DateTime(2024, 12, 5), 15000, testProject3);
            var testTask6 = new ProjectTask("Testiranje sustava", "Testiranje svih komponenti", new DateTime(2025, 1, 20), 9000, testProject3);

            allProjects[testProject1] = new List<ProjectTask> { testTask1, testTask2 };
            allProjects[testProject2] = new List<ProjectTask> { testTask3 };
            allProjects[testProject3] = new List<ProjectTask> { testTask4, testTask5, testTask6 };

            testProject3.OnHold();

            MainMenu();
        }

        static void MainMenu()
        {

            while (true)
            {
                Console.WriteLine("1 - Ispis svih projekata s pripadajućim zadacima\n2 - Dodavanje novog projekta\n3 - Brisanje projekta\n4 - Prikaz svih zadataka s rokom u sljedećih 7 dana\n5 - Prikaz  projekata filtriranih po status\n6 - Upravljanje pojedinim projektom\n7 - Upravljanje pojedinim zadatkom");
                var menuSelection = Console.ReadLine();

                switch (menuSelection)
                {
                    case "1":
                        PrintAllProjects();
                        break;
                    case "2":
                        AddProject();
                        break;
                    case "3":
                        DeleteProject();
                        break;
                    case "4":
                        SevenDayDeadlineTasks();
                        break;
                    case "5":
                        FilterProjectsByStatus();
                        break;
                    default:
                        Console.WriteLine("Krivi unos, unesi ponovno!");
                        break;
                }
            }
        }

        static void PrintAllProjects()
        {
            Console.Clear();

            foreach (var project in allProjects)
            {
                Console.WriteLine($"Project: {project.Key.Name} Status: {project.Key.Status} ");
                foreach (var task in project.Value)
                {
                    Console.WriteLine($"Task: {task.Name} - Status: {task.Status}, Deadline: {task.Deadline} ");
                }
                Console.WriteLine();
            }
        }

        static void AddProject()
        {
            var projectName = CheckEmptyStringAndSpecialChars("ime");
            var projectDescription = CheckEmptyStringAndSpecialChars("opis");

            var projectStart = CheckDate("pocetka");
            var projectDeadline = CheckDate("roka");

            while(DateTime.Compare(projectStart, projectDeadline) >= 0)
            {
                Console.WriteLine("Deadline ne moze bit prije pocetka projekta, unesi datum roka ponovno!");
                projectDeadline = CheckDate("roka");
            }

            Project newProject = new Project(projectName, projectDescription, projectStart, projectDeadline);

            allProjects.Add(newProject, new List<ProjectTask>()); 
        }

        static string CheckEmptyStringAndSpecialChars(string attribute)
        {
            var input = string.Empty;
            while (true)
            {
                Console.Write($"Unesi {attribute} projekta: ");
                input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Ne smije biti empty string! ");
                    continue;
                }
                else if(!input.All(char.IsLetter))
                {
                    Console.WriteLine("Smijes unit samo slova!");
                    continue;
                }

                break;
            }

            return input;
        }

        static DateTime CheckDate(string typeOfDate)
        {
            DateTime dateOutput;

            while (true)
            {
                Console.Write($"Unesi datum {typeOfDate}(yyyy-MM-dd): ");
                var input = Console.ReadLine();

                if(!DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOutput))
                {
                    Console.WriteLine("Krivi unos, unesi datum u formatu (yyyy-MM-dd)!");
                    continue;
                }

                break;
            }

            return dateOutput;
        }

        static void DeleteProject()
        {
            Console.Clear();

            PrintAllProjects();

            Console.Write("Unesi ime projekta koji zelis izbrisat: ");
            var projectToDelete = Console.ReadLine();

            var projectFound = false;

            foreach(var project in allProjects)
            {
                if(project.Key.Name == projectToDelete)
                {
                    projectFound = true;

                    Console.Write("Jesi li siguran da zelis obrisat ovaj projekt? (da, ne): ");
                    var confirmation = Console.ReadLine();

                    while(confirmation != "da" && confirmation != "ne")
                    {
                        Console.WriteLine("Krivi unos, unesi da ili ne!");
                        confirmation = Console.ReadLine();
                    }

                    if(confirmation == "da")
                    {
                        allProjects.Remove(project.Key);
                    }
                }
            }

            if (!projectFound)
            {
                Console.WriteLine("Taj projekt ne postoji!");
            }
        }

        static void SevenDayDeadlineTasks()
        {
            Console.Clear();

            foreach(var project in allProjects)
            {
                foreach(var task in project.Value)
                {
                    if ((task.Deadline - DateTime.Now).TotalDays <= 7)
                    {
                        Console.WriteLine($"Task: {task.Name} - Deadline: {task.Deadline}, Project: {task.ProjectName.Name }");
                    }
                }
            }
        }

        static void FilterProjectsByStatus()
        {
            Console.Clear();

            Console.WriteLine("Unesi status za ispis svih projekata s istim statusom");
            while (true)
            {
                Console.WriteLine("1 - Aktivni\n2 - Na cekanju\n3 - Zavrseni");
                var statusSelection = Console.ReadLine();

                switch (statusSelection)
                {
                    case "1":
                        PrintByStatus("Active");
                        break;
                    case "2":
                        PrintByStatus("OnHold");
                        break;
                    case "3":
                        PrintByStatus("Finished");
                        break;
                    default:
                        Console.WriteLine("Krivi unos, unesi ponovno!");
                        break;
                }
            }
        }

        static void PrintByStatus(string statusAttribute)
        {
            foreach(var project in allProjects)
            {
                if(project.Key.Status.ToString() == statusAttribute)
                {
                    Console.WriteLine($"Project: {project.Key.Name} - Status: {project.Key.Status}");
                }
            }
        }
    }
}
