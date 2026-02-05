namespace OOP_Fundamentals_Library
{
    public class Employee
    {
        public string Name;
        public int Age;
        public decimal Salary;
        public string Position;

        public void PrintInfo()
        {
            Console.WriteLine($"Employee: {Name}, {Age} years old");
        }

        public void IncreaseSalary(decimal amount)
        {
            Salary += amount;
        }

        public void ProcessPayroll()
        {
            Console.WriteLine($"Processing payroll for {Name}: {Salary}");
        }
    }
}
