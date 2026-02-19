using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLID_Fundamentals
{
    // Separate Customer class
    public class Customer
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
    }
}
