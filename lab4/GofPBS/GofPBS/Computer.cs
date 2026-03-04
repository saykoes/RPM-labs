using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GofPBS
{
    public class Computer
    {
        public string? CPU;
        public int? RAM;
        public string? GPU;
        public List<string> AdditionalComponents = new List<string>();

        public void Display()
        {
            Console.WriteLine("=== Computer Config ===");
            Console.WriteLine($"CPU: {CPU ?? "Not Specified"}");
            Console.WriteLine($"RAN: {RAM} GB");
            Console.WriteLine($"GPU: {GPU ?? "None"}");
            Console.WriteLine($"Add. Components: {(AdditionalComponents.Count > 0 ? string.Join(", ", AdditionalComponents) : "none")}");
            Console.WriteLine("=======================");
        }

        public Computer ShallowCopy()
        {
            return (Computer)MemberwiseClone();
        }

        public Computer DeepCopy()
        {
            Computer clone = (Computer)MemberwiseClone();
            if (AdditionalComponents is not null)
                clone.AdditionalComponents = new List<string>(this.AdditionalComponents);
            return clone;
        }
    }
}
