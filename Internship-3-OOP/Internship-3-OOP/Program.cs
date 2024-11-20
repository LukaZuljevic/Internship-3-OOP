using System.Globalization;
using System.Net.WebSockets;
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
            testProject3.OnHold();

            var testTask1 = new ProjectTask("Dizajn pocetne stranice", "Izrada dizajna", new DateTime(2024, 11, 20), 10000, testProject1);
            testTask1.Finished();
            var testTask2 = new ProjectTask("Implementacija featurea", "Novi feature", new DateTime(2024, 12, 15), 12000, testProject1);
            testTask2.LowPriority();
            var testTask3 = new ProjectTask("Projektna specifikacija", "Izrada detaljne specifikacije za projekt", new DateTime(2024, 11, 23), 8000, testProject2);
            var testTask4 = new ProjectTask("Planiranje projekta", "Postavljanje planova i ciljeva", new DateTime(2024, 11, 15), 7000, testProject3);
            testTask4.MediumPriority();
            var testTask5 = new ProjectTask("Kodiranje modula", "Implementacija osnovnih funkcionalnosti", new DateTime(2024, 12, 5), 15000, testProject3);
            testTask5.LowPriority();
            var testTask6 = new ProjectTask("Testiranje sustava", "Testiranje svih komponenti", new DateTime(2025, 1, 20), 9000, testProject3);
            testTask6.Delayed();

            allProjects[testProject1] = new List<ProjectTask> { testTask1, testTask2 };
            allProjects[testProject2] = new List<ProjectTask> { testTask3 };
            allProjects[testProject3] = new List<ProjectTask> { testTask4, testTask5, testTask6 };

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
                    case "7":
                        TaskMenu();
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
                Console.WriteLine("1 - Ispis svih zadataka unutar odabranog projekta\n2 - Prikaz detalja odabranog projekta\n3 - Uređivanje statusa projekta\n4 - Dodavanje zadatka unutar projekta\n5 - Brisanje zadatka iz projekta\n6 - Prikaz ukupno očekivanog vremena potrebnog za sve aktivne zadatke u projektu\n7 - Prikaz zadataka po duljini trajanja\n8 - Prikaz zadatka sortirani po prioritetu\n9 - Vrati se na main menu");
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
                    case "6":
                        pickedProject = PickProject();
                        TimeToFinishTasks(pickedProject);
                        break;
                    case "7":
                        pickedProject = PickProject();
                        SortTasksByDuration(pickedProject);
                        break;
                    case "8":
                        pickedProject = PickProject();
                        SortTasksByPriority(pickedProject);
                        break;
                    case "9":
                        return;
                    default:
                        Console.WriteLine("Krivi unos, unesi ponovno!");
                        break;
                }
            }
        }

        static void TaskMenu()
        {
            Console.Clear();

            while (true)
            {
                Console.WriteLine("1 - Prikaz detalja odabranog zadatka\n2 - Uređivanje statusa zadatka\n3 - Vrati se na main menu");
                var menuSelection = Console.ReadLine();

                Project pickedProject;
                ProjectTask pickedTask;

                switch (menuSelection)
                {
                    case "1":
                        pickedProject = PickProject();
                        pickedTask = PickTask(pickedProject);
                        PrintTaskDetails(pickedTask);
                        break;
                    case "2":
                        pickedProject = PickProject();
                        pickedTask = PickTask(pickedProject);
                        EditTaskStatus(pickedTask);
                        IsProjectFinished(pickedProject);//provjerava jesu svi zadaci finished -> ako jesu projekt se stavlja pod finished isto
                        break;
                    case "3":
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
                Console.WriteLine($"Ime: {project.Key.Name} Status: {project.Key.Status} ");
                foreach (var task in project.Value)
                {
                    Console.WriteLine($"       Task - {task.Name}");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static void AddProject()
        {
            Console.Clear();

            var projectName = CheckEmptyStringAndSpecialChars("ime", "projekta");

            while(IsProjectNameDuplicate(projectName))
            {
                Console.WriteLine("Ime projekta vec postoji!");

                projectName = CheckEmptyStringAndSpecialChars("ime", "projekta");
                IsProjectNameDuplicate(projectName);
            }

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

            Console.WriteLine("Projekt uspjesno dodan!\n");
        }

        static bool IsProjectNameDuplicate(string projectName)
        {
            foreach (var project in allProjects.Keys)
            {
                if (string.Equals(project.Name, projectName, StringComparison.OrdinalIgnoreCase))
                    return true;
                
            }
            return false;
        }

        static string CheckEmptyStringAndSpecialChars(string attribute, string entity)
        {
            var input = string.Empty;
            while (true)
            {
                Console.Write($"Unesi {attribute} {entity}: ");
                input = Console.ReadLine().Trim();

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
                if (string.Equals(project.Key.Name, projectToDelete, StringComparison.OrdinalIgnoreCase))
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
                        allProjects.Remove(project.Key);
                }
            }

            if(!projectFound)
                Console.WriteLine("Taj projekt ne postoji!\n");
        }

        static void SevenDayDeadlineTasks()
        {
            Console.Clear();

            foreach (var project in allProjects)
            {
                foreach (var task in project.Value)
                {
                    if ((task.Deadline - DateTime.Now).TotalDays <= 7)
                        Console.WriteLine($"Task: {task.Name} - Rok: {task.Deadline}, Projekt: {task.ProjectName.Name}\n");
                }
            }
            Console.WriteLine();
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
            Console.Clear();

            var isEmpty = true;
            foreach (var project in allProjects)
            {
                if (project.Key.Status.ToString() == statusAttribute)
                {
                    isEmpty = false;
                    Console.WriteLine($"Ime: {project.Key.Name} - Status: {project.Key.Status}\n");
                }
            }
            if (isEmpty)
                Console.WriteLine("Nema projekata sa takvim statusom!\n");
        }

        static Project PickProject()
        {
            Console.Clear();

            foreach (var project in allProjects)
            {
                Console.WriteLine($"Projekt: {project.Key.Name}");
            }

            Project selectedProject;

            do
            {
                Console.Write("Unesi jedan od ponudenih projekata: ");
                var projectPick = Console.ReadLine();

                selectedProject = allProjects.Keys.FirstOrDefault(proj => proj.Name.ToLower().Trim() == projectPick.ToLower().Trim());

                if (selectedProject == null)
                    Console.WriteLine("Taj projekt ne postoji, unesi ponovno!");

            } while (selectedProject == null);

            return selectedProject;
        }

        static ProjectTask PickTask(Project pickedProject)
        {
            Console.Clear();

            PrintTasksByProject(pickedProject);

            ProjectTask selectedTask;

            do
            {
                Console.Write("Unesi jedan od zadataka: ");
                var taskPick = Console.ReadLine();

                selectedTask = allProjects[pickedProject].FirstOrDefault(task => task.Name.ToLower().Trim() == taskPick.ToLower().Trim());

                if (selectedTask == null)
                    Console.WriteLine("Taj zadatak ne postoji, unesi ponovno!");

            } while (selectedTask == null);

            return selectedTask;
        }

        static void PrintTasksByProject(Project pickedProject)
        {
            Console.Clear();

            var hasTasks = true;
            Console.WriteLine($"Zadaci za taj projekt:\n");
            foreach (var task in allProjects[pickedProject])
            {
                Console.WriteLine($"Task: {task.Name} - {task.Description}, Status: {task.Status}\n");
                hasTasks = false;
            }

            if(hasTasks)
                Console.WriteLine("Ovaj projekt jos nema zadataka!\n");
        }

        static void PrintProjectDetails(Project pickedProject)
        {
            Console.Clear();

            Console.WriteLine($"Detaljni prikaz projekta: \n");
            Console.WriteLine($"Ime: {pickedProject.Name}\nOpis: {pickedProject.Description}\nPocetak: {pickedProject.StartOfProject}\nRok: {pickedProject.EndOfProject}\nStatus: {pickedProject.Status}\n");
        }

        static void EditProjectStatus(Project pickedProject)
        {
            Console.Clear();

            if (!CanEditProject(pickedProject))
                return;

            Console.WriteLine("(Upozorenje)Ako oznacis projekt kao finished, automatski ce se svi zadaci unutar projekta oznacit kao finished\n");    
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

                        foreach(var task in allProjects[pickedProject])
                        {
                            if(task.Status.ToString() != "Finished")
                                task.Finished();
                        }
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

            if (!CanEditProject(pickedProject))
                return;

            Console.WriteLine("Dodaj zadatak unutar odabranog projekta");

            var taskName = CheckEmptyStringAndSpecialChars("ime", "zadatka");

            while (IsTaskNameDuplicate(allProjects[pickedProject], taskName))
            {
                Console.WriteLine("Ime zadatka vec postoji!");
                taskName = CheckEmptyStringAndSpecialChars("ime", "zadatka");
                IsTaskNameDuplicate(allProjects[pickedProject], taskName);
            }

            var taskDescription = CheckEmptyStringAndSpecialChars("opis", "zadatka");
            var taskDeadline = CheckDate("rok");

            int expectedDurationToFinish;

            Console.Write("Unesi ocekivano vrijeme trajanja zadatka (u minutama): ");
            while (!int.TryParse(Console.ReadLine(), out expectedDurationToFinish) || expectedDurationToFinish <= 0)
            {
                Console.Write("Unesi pozitivan broj: ");
            }
            Console.WriteLine("Uspjesno dodan novi zadatak!\n");

            var newTask = new ProjectTask(taskName, taskDescription, taskDeadline, expectedDurationToFinish, pickedProject);

            allProjects[pickedProject].Add(newTask);
        }

        static bool IsTaskNameDuplicate(List<ProjectTask> pickedProjectTasks, string taskName)
        {
            foreach(var task in pickedProjectTasks)
            {
                if(string.Equals(task.Name, taskName , StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        static void DeleteTaskFromProject(Project pickedProject)
        {
            Console.Clear();

            if (!CanEditProject(pickedProject))
                return;

            PrintTasksByProject(pickedProject);

            Console.Write("Unesi ime zadatka koji zelis izbrisat: ");
            var taskToDelete = Console.ReadLine();

            var taskFound = false;

            foreach (var task in allProjects[pickedProject])
            {
                if (string.Equals(task.Name, taskToDelete, StringComparison.OrdinalIgnoreCase))
                {
                    taskFound = true;

                    Console.Write("Jesi li siguran da zelis obrisat ovaj zadatak? (da, ne): ");
                    var confirmation = Console.ReadLine();

                    while (confirmation != "da" && confirmation != "ne")
                    {
                        Console.WriteLine("Krivi unos, unesi da ili ne!");
                        confirmation = Console.ReadLine();
                    }

                    if (confirmation == "da") {
                        allProjects[pickedProject].Remove(task);
                        Console.WriteLine("Uspjesno izbrisan zadatak!\n");
                        break;
                    }
                }
            }

            if (!taskFound)
                Console.WriteLine("Taj zadatak ne postoji!");
        }

        static void TimeToFinishTasks(Project pickedProject)
        {
            Console.Clear();

            double timeToFinish = 0;

            foreach (var task in allProjects[pickedProject])
            {
                if (task.Status.ToString() == "Active")
                    timeToFinish += task.ExpectedDuration;
            }

            TimeSpan timeSpan = TimeSpan.FromMinutes(timeToFinish);

            int days = timeSpan.Days;
            int hours = timeSpan.Hours;
            int remainingMinutes = timeSpan.Minutes;

            Console.WriteLine($"Vrime potrebno za zavrsit: {days} dana, {hours} sati i {remainingMinutes} minuta.\n");
        }

        static void PrintTaskDetails(ProjectTask pickedTask)
        {
            Console.Clear();
            Console.WriteLine($"Task: {pickedTask.Name} - {pickedTask.Description}, Rok: {pickedTask.Deadline}, Status: {pickedTask.Status}, Ocekivani trajanje: {pickedTask.ExpectedDuration} minuta\n");
        }

        static void EditTaskStatus(ProjectTask pickedTask)
        {
            Console.Clear();

            if (pickedTask.Status.ToString() == "Finished")
            {
                Console.WriteLine("Ne mozes editat gotov zadatak!");
                return;
            }

            Console.WriteLine("Novi status zadatka\n");

            while (true)
            {
                Console.WriteLine("1 - Active\n2 - Finished\n3 - Delayed");
                var statusSelection = Console.ReadLine();

                switch (statusSelection)
                {
                    case "1":
                        pickedTask.Active();
                        return;
                    case "2":
                        pickedTask.Finished();
                        return;
                    case "3":
                        pickedTask.Delayed();
                        return;
                    default:
                        Console.WriteLine("Krivi unos, unesi ponovno!");
                        break;
                }
            }
        }

        static void IsProjectFinished(Project project)
        {
            if (allProjects.TryGetValue(project, out var tasks))
            {
                foreach (var task in tasks)
                {
                    if(task.Status.ToString() != "Finished")
                    {
                        return;
                    }
                }

                project.Finished();
            }
        }

        static void SortTasksByDuration(Project pickedProject)
        {
            Console.Clear();

            var sortedTasks = allProjects[pickedProject].OrderBy(t => t.ExpectedDuration).ToList();

            Console.WriteLine("Prikaz zadataka po duljini trajanja\n");

            foreach(var task in sortedTasks)
            {
                TimeSpan timeSpan = TimeSpan.FromMinutes(task.ExpectedDuration);

                int days = timeSpan.Days;
                int hours = timeSpan.Hours;
                int remainingMinutes = timeSpan.Minutes;

                Console.WriteLine($"{task.Name} - {task.Description}, Ocekivano trajanje:  {days} dana, {hours} sati i {remainingMinutes} minuta, Status:{task.Status}\n");
            }
        }

        static void SortTasksByPriority(Project pickedProject)
        {
            Console.Clear();
            var highPriorityTasks = allProjects[pickedProject].Where(t => t.Priority.ToString() == "High");
            var mediumPriorityTasks = allProjects[pickedProject].Where(t => t.Priority.ToString() == "Medium");
            var lowPriorityTasks = allProjects[pickedProject].Where(t => t.Priority.ToString() == "Low");

            Console.WriteLine("High prioritet zadaci:");
            foreach (var task in highPriorityTasks)
            {
                Console.WriteLine($"Task: {task.Name}, Ocekivano trajanje: {task.ExpectedDuration} minuta");
            }

            Console.WriteLine("\nMedium prioritet zadaci:");
            foreach (var task in mediumPriorityTasks)
            {
                Console.WriteLine($"Task: {task.Name}, Ocekivano trajanje: {task.ExpectedDuration} minuta");
            }

            Console.WriteLine("\nLow prioritet zadaci:");
            foreach (var task in lowPriorityTasks)
            {
                Console.WriteLine($"Task: {task.Name}, Ocekivano trajanje: {task.ExpectedDuration} minuta");
            }

            Console.WriteLine();
        }

        static bool CanEditProject(Project pickedProject)
        {
            if (pickedProject.Status.ToString() == "Finished")
            {
                Console.WriteLine("Ne mozes editat gotov projekt!");
                return false;
            }
            return true;
        }
    }
}
