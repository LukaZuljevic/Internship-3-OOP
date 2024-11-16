using System;
using Internship_3_OOP.Classes;
using Internship_3_OOP.Enum;

namespace Internship_3_OOP
{
    internal class Program
    {
        static Dictionary<Project, List<ProjectTask>> allProjects = new Dictionary<Project, List<ProjectTask>>();

        static void Main(string[] args)
        {
            var testProject1 = new Project("Projekt A", "Prvi test projekt", new DateTime(2024,11,1), new DateTime(2024, 12, 31));
            var testProject2 = new Project("Projekt B", "Drugi test projekt", new DateTime(2024, 10, 1), new DateTime(2024, 11, 30));

            var testTask1 = new ProjectTask("Dizajn pocetne stranice", "Izrada dizajna", new DateTime(2024, 11, 20), 10000, testProject1);
            var testTask2 = new ProjectTask("Implementacija featurea", "Novi feature", new DateTime(2024, 12, 15), 12000, testProject1);
            var testTask3 = new ProjectTask("Projektna specifikacija", "Izrada detaljne specifikacije za projekt", new DateTime(2024, 11, 25), 8000, testProject2);

            allProjects[testProject1] = new List<ProjectTask> { testTask1, testTask2 };
            allProjects[testProject2] = new List<ProjectTask> { testTask3 };

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
                    default:
                        Console.WriteLine("Krivi unos, unesi ponovno!");
                        break;
                }
            }
        }

        static void PrintAllProjects()
        {
            Console.Clear();

            foreach(var project in allProjects)
            {
                Console.WriteLine($"Project: {project.Key.Name} Status: {project.Key.Status} ");
                foreach(var task in project.Value)
                {
                    Console.WriteLine($"Task: {task.Name} - Status: {task.Status}, Deadline: {task.Deadline} ");
                }
                Console.WriteLine();
            }
        }
    }
}
