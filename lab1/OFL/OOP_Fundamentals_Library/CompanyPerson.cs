using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Fundamentals_Library
{
    public abstract class CompanyPerson : Person, IReportable
    {
        private decimal _salary;
        private int _years;
        private bool _hasCertification;

        public decimal Salary
        {
            get { return _salary; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Salary cannot be negative");
                _salary = value;
            }
        }

        public virtual int Years
        {
            get { return _years; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Experience cannot be negative");
                if (value > Age - 10)
                    throw new ArgumentException("Experience is too long for the age");
                _years = value;
            }
        }
        public bool HasCertification
        {
            get { return _hasCertification; }
            set { _hasCertification = value; }
        }
        public abstract decimal BonusMultiplier { get; }
        public abstract decimal SalaryIncrease { get; }

        public virtual string ReportString => $"{GetType().Name} Report:\n  Name: {Name}";

        public virtual string GenerateReport()
        {
            return ReportString;
        }
    }
}
