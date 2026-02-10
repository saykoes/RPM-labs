using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Swift;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Fundamentals_Library
{
    public abstract class Person
    {
        private string _name = "Unknown";
        private int _age;


        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be empty");
                if (value.Length > 255)
                    throw new ArgumentException("Name is too long");
                _name = value;
            }
        }

        public virtual int Age
        {
            get { return _age; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Age cannot be negative");
                if (value > 150)
                    throw new ArgumentException("Age is too high");
                _age = value;
            }
        }

        public virtual void PrintInfo()
        {
            Console.WriteLine($"{GetType().Name}: {Name}, {Age} years old");
        }
    }
}
