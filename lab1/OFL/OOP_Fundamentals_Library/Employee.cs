namespace OOP_Fundamentals_Library
{
    public class Employee : CompanyPerson
    {
        private string _position = "Unknown";
        public string Position
        {
            get { return _position; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Position cannot be empty");
                if (value.Length > 100)
                    throw new ArgumentException("Position too long");
                _position = value;
            }
        }
        public override int Age
        {
            get { return base.Age; }
            set
            {
                if (value < 18)
                    throw new ArgumentException("Employee can't be under 18");
                base.Age = value;
            }
        }
        public override decimal BonusMultiplier => 0.1m;
        public override decimal SalaryIncrease => 1000m;
        public override string ReportString => $"{base.ReportString}\n  Age: {Age}\n  Salary: {Salary}";

        public void IncreaseSalary(decimal amount)
        {
            Salary += amount;
        }

    }
}
