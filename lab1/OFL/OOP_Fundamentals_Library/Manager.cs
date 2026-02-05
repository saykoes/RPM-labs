namespace OOP_Fundamentals_Library
{
    public class Manager
    {
        public string Name;
        public int Age;
        public decimal Salary;
        public string Department;
        public List<Employee> Team = new();

        public void PrintInfo()
        {
            Console.WriteLine($"Manager: {Name}, {Age} years old, Department: {Department}");
        }

        public void AssignTaskToEmployee(Employee emp, string task)
        {
            Console.WriteLine($"Assigning task '{task}' to {emp.Name}");
        }
    }
}
