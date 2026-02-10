using OOP_Fundamentals_Library;

internal class Program
{
    static void Main(string[] args)
    {
        
        var customer = new Customer
        {
            Name = "John",
            Age = 30
        };

        var employee = new Employee
        {
            Name = "Alice",
            Age = 25,
            Salary = 50000,
            Position = "Developer"
        };

        var manager = new Manager
        {
            Name = "Bob",
            Age = 40,
            Salary = 80000,
            Department = "IT"
        };

        manager.AddToTeam(employee);

        employee.Salary = 55000;

        customer.PrintInfo();
        employee.PrintInfo();
        manager.PrintInfo();

        var payroll = new PayrollSystem();

        payroll.ProcessSalary(employee);
        payroll.ProcessSalary(manager);

        var report = new ReportService();

        report.GenerateReport(employee);
        report.GenerateReport(manager);

    }
}