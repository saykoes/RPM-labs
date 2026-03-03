using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GofPBS
{
    public class ComputerBuilder
    {
        private Computer _computer = new Computer();

        public ComputerBuilder WithCPU(string cpu)
        {
            _computer.CPU = cpu;
            return this;
        }

        public ComputerBuilder WithRAM(int ram)
        {
            _computer.RAM = ram;
            return this;
        }

        public ComputerBuilder WithGPU(string gpu)
        {
            _computer.GPU = gpu;
            return this;
        }

        public ComputerBuilder WithComponent(string component)
        {
            _computer.AdditionalComponents.Add(component);
            return this;
        }

        public Computer Build()
        {
            // storing _computer in a new object and returning it
            // because "return _computer;" would only return a reference to the _computer
            // and if we would run the method again, it would modify the original _computer which reference we returned here
            Computer finishedComputer = _computer;
            _computer = new Computer(); 
            return finishedComputer;
        }
    }
}
