using System.Xml.Linq;

namespace OOP_Fundamentals_Library
{
    public class PayrollSystem
    {
        public void ProcessSalary(CompanyPerson emp)
        {
            Console.WriteLine($"Processing salary for {GetType().Name} {emp.Name}: {emp.Salary}");
            emp.Salary += emp.SalaryIncrease;
        }

        public decimal CalculateBonus(CompanyPerson emp)
        {
            decimal bonus = emp.Salary * emp.BonusMultiplier;

            if (emp.Years > 5)
                bonus += 500;

            if (emp.HasCertification)
                bonus += 300;

            return bonus;
        }

        public void ProcessPayroll(Employee emp)
        {
            Console.WriteLine($"Processing payroll for {emp.Name}: {emp.Salary}");
        }
    }
}
