namespace OOP_Fundamentals_Library
{
    public class ReportService
    {
        public static void GenerateEmployeeReport(Employee emp)
        {
            Console.WriteLine($"Employee Report:");
            Console.WriteLine($"  Name: {emp.Name}");
            Console.WriteLine($"  Age: {emp.Age}");
            Console.WriteLine($"  Salary: {emp.Salary}"); // Прямой доступ к полю
        }

        public static void GenerateManagerReport(Manager mgr)
        {
            Console.WriteLine($"Manager Report:");
            Console.WriteLine($"  Name: {mgr.Name}");
            Console.WriteLine($"  Department: {mgr.Department}");
            Console.WriteLine($"  Team Size: {mgr.Team.Count}"); // Прямой доступ к полю
        }
    }
}
