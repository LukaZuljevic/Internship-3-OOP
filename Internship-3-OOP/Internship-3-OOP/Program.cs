using System.Globalization;
using Internship_3_OOP.Classes;

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
                Console.WriteLine("1 - Ispis svih projekata\n2 - Dodavanje novog projekta\n3 - Brisanje projekta\n4 - Prikaz svih zadataka s rokom u sljedećih 7 dana\n5 - Prikaz  projekata filtriranih po status\n6 - Upravljanje pojedinim projektom\n7 - Upravljanje pojedinim zadatkom");
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
                    case "6":
                        ProjectMenu();
                        break;
                    default:
                        Console.WriteLine("Krivi unos, unesi ponovno!");
                        break;
                }
            }
        }

        static void ProjectMenu()
        {
            Console.Clear();

            while (true)
            {
                Console.WriteLine("1 - Ispis svih zadataka unutar odabranog projekta\n2 - Prikaz detalja odabranog projekta\n3 - Uređivanje statusa projekta\n4 - Dodavanje zadatka unutar projekta\n5 - Brisanje zadatka iz projekta\n6 - Prikaz ukupno očekivanog vremena potrebnog za sve aktivne zadatke u projektu\n7 - Vrati se na main menu");
                var menuSelection = Console.ReadLine();

                Project pickedProject;

                switch (menuSelection)
                {
                    case "1":
                        pickedProject = PickProject();
                        PrintTasksByProject(pickedProject);
                        break;
                    case "2":
                        pickedProject = PickProject();
                        PrintProjectDetails(pickedProject);
                        break;
                    case "3":
                        pickedProject = PickProject();
                        EditProjectStatus(pickedProject);
                        break;
                    case "4":
                        pickedProject = PickProject();
                        AddTaskToProject(pickedProject);
                        break;
                    case "5":
                        pickedProject = PickProject();
                        DeleteTaskFromProject(pickedProject);
                        break;
                    case "7":
                        return;
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
                Console.WriteLine($"Projekt: {project.Key.Name} Status: {project.Key.Status} ");
            }
        }

        static void AddProject()
        {
            var projectName = CheckEmptyStringAndSpecialChars("ime", "projekta");
            var projectDescription = CheckEmptyStringAndSpecialChars("opis", "projekta");

            var projectStart = CheckDate("pocetka");
            var projectDeadline = CheckDate("roka");

            while (DateTime.Compare(projectStart, projectDeadline) >= 0)
            {
                Console.WriteLine("Deadline ne moze bit prije pocetka projekta, unesi datum roka ponovno!");
                projectDeadline = CheckDate("roka");
            }

            Project newProject = new Project(projectName, projectDescription, projectStart, projectDeadline);

            allProjects.Add(newProject, new List<ProjectTask>());
        }

        static string CheckEmptyStringAndSpecialChars(string attribute, string entity)
        {
            var input = string.Empty;
            while (true)
            {
                Console.Write($"Unesi {attribute} {entity}: ");
                input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Ne smije biti empty string! ");
                    continue;
                }
                else if (!input.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
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

                if (!DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOutput))
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

            foreach (var project in allProjects)
            {
                if (project.Key.Name.ToLower() == projectToDelete.ToLower())
                {
                    projectFound = true;

                    Console.Write("Jesi li siguran da zelis obrisat ovaj projekt? (da, ne): ");
                    var confirmation = Console.ReadLine();

                    while (confirmation != "da" && confirmation != "ne")
                    {
                        Console.WriteLine("Krivi unos, unesi da ili ne!");
                        confirmation = Console.ReadLine();
                    }

                    if (confirmation == "da")
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

            foreach (var project in allProjects)
            {
                foreach (var task in project.Value)
                {
                    if ((task.Deadline - DateTime.Now).TotalDays <= 7)
                    {
                        Console.WriteLine($"Task: {task.Name} - Rok: {task.Deadline}, Projekt: {task.ProjectName.Name}");
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
                Console.WriteLine("1 - Active\n2 - On hold\n3 - Finished");
                var statusSelection = Console.ReadLine();

                switch (statusSelection)
                {
                    case "1":
                        PrintByStatus("Active");
                        return;
                    case "2":
                        PrintByStatus("OnHold");
                        return;
                    case "3":
                        PrintByStatus("Finished");
                        return;
                    default:
                        Console.WriteLine("Krivi unos, unesi ponovno!");
                        break;
                }
            }
        }

        static void PrintByStatus(string statusAttribute)
        {
            foreach (var project in allProjects)
            {
                if (project.Key.Status.ToString() == statusAttribute)
                {
                    Console.WriteLine($"Projekt: {project.Key.Name} - Status: {project.Key.Status}");
                }
            }
        }

        static Project PickProject()
        {
            Console.Clear();

            foreach (var project in allProjects)
            {
                Console.WriteLine($"Projekt: {project.Key.Name}");
            }

            string projectPick;
            Project selectedProject = null;

            do
            {
                Console.Write("Unesi jeadn od projekata: ");
                projectPick = Console.ReadLine();

                selectedProject = allProjects.Keys.FirstOrDefault(proj => proj.Name.ToLower() == projectPick.ToLower());

                if (selectedProject == null)
                {
                    Console.WriteLine("Krivi unos, unesi ponovno!");
                }
            } while (selectedProject == null);

            return selectedProject;
        }

        static void PrintTasksByProject(Project pickedProject)
        {
            if (allProjects.TryGetValue(pickedProject, out var tasks))
            {
                Console.WriteLine($"Zadaci za taj projekt:");
                foreach (var task in tasks)
                {
                    Console.WriteLine($"Task: {task.Name} - {task.Description}, Rok: {task.Deadline}, Status: {task.Status}");
                }
            }

        }

        static void PrintProjectDetails(Project pickedProject)
        {
            Console.Clear();

            Console.WriteLine($"Detaljni prikaz projekta: ");
            Console.WriteLine($"Ime: {pickedProject.Name}\nOpis: {pickedProject.Description}\nPocetak: {pickedProject.StartOfProject}\nRok: {pickedProject.EndOfProject}\nStatus: {pickedProject.Status}");
        }

        static void EditProjectStatus(Project pickedProject)
        {
            Console.Clear();
            Console.WriteLine("Novi status projekta");

            while (true)
            {
                Console.WriteLine("1 - Active\n2 - On hold\n3 - Finished");
                var statusSelection = Console.ReadLine();

                switch (statusSelection)
                {
                    case "1":
                        pickedProject.Active();
                        return;
                    case "2":
                        pickedProject.OnHold();
                        return;
                    case "3":
                        pickedProject.Finished();
                        return;
                    default:
                        Console.WriteLine("Krivi unos, unesi ponovno!");
                        break;
                }
            }
        }

        static void AddTaskToProject(Project pickedProject)
        {
            Console.Clear();

            Console.WriteLine("Dodaj zadatak unutar odabranog projekta");

            var taskName = CheckEmptyStringAndSpecialChars("ime", "zadatka");
            var taskDescription = CheckEmptyStringAndSpecialChars("opis", "zadatka");

            var taskDeadline = CheckDate("rok");

            Console.Write("Unesi ocekivano vrijeme trajanja zadatka(u minutama): ");
            var expectedDurationToFinish = int.Parse(Console.ReadLine());

            while(expectedDurationToFinish <= 0)
            {
                Console.Write("Unesi pozitivan broj: ");
                expectedDurationToFinish = int.Parse(Console.ReadLine());
            }

            var newTask = new ProjectTask(taskName, taskDescription, taskDeadline, expectedDurationToFinish, pickedProject);

            allProjects[pickedProject].Add(newTask);
        }

        static void DeleteTaskFromProject(Project pickedProject)
        {
            Console.Clear();

            PrintTasksByProject(pickedProject);

            Console.Write("Unesi ime zadatka koji zelis izbrisat: ");
            var taskToDelete = Console.ReadLine();

            var taskFound = false;

            var tasksToRemove = new List<ProjectTask>();

            foreach (var task in allProjects[pickedProject])
            {
                if (task.Name.ToLower() == taskToDelete.ToLower())
                {
                    taskFound = true;

                    Console.Write("Jesi li siguran da zelis obrisat ovaj zadatak? (da, ne): ");
                    var confirmation = Console.ReadLine();

                    while (confirmation != "da" && confirmation != "ne")
                    {
                        Console.WriteLine("Krivi unos, unesi da ili ne!");
                        confirmation = Console.ReadLine();
                    }

                    if (confirmation == "da")
                    {
                        tasksToRemove.Add(task);
                    }
                }
            }

            foreach (var task in tasksToRemove)
            {
                allProjects[pickedProject].Remove(task);
            }

            if (!taskFound)
            {
                Console.WriteLine("Taj zadatak ne postoji!");
            }
        }
    }
}
