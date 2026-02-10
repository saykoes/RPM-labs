using System.Threading.Tasks;

namespace OOP_Fundamentals_Library
{
    public class Manager : CompanyPerson
    {
        
        private string _department = "Unknown";

        private readonly List<Employee> _team = new();

        public IReadOnlyList<Employee> Team => _team.AsReadOnly();

        public void AddToTeam(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));
            if (_team.Contains(employee))
                throw new InvalidOperationException("Employee already in team");
            _team.Add(employee);
        }

        public bool RemoveFromTeam(Employee employee)
        {
            return _team.Remove(employee);
        }

        public string Department
        {
            get { return _department; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Department cannot be empty");
                _department = value;
            }
        }

        public override decimal BonusMultiplier => 0.2m;
        public override decimal SalaryIncrease => 2000m;
        public override string ReportString => $"{base.ReportString}\n  Department: {Department}\n  Team Size: {Team.Count}";



        public override void PrintInfo()
        {
            Console.WriteLine($"Manager: {Name}, {Age} years old, Department: {Department}");
        }

        public void AssignTaskToEmployee(Employee emp, string task)
        {
            Console.WriteLine($"Assigning task '{task}' to {emp.Name}");
        }
    }
}
