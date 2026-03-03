using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GofPBS
{
    public interface IComputerFactory
    {
        public Computer CreateComputer();
    }
    public class OfficeComputerFactory : IComputerFactory
    {
        public Computer CreateComputer()
        {
            return new ComputerBuilder()
                .WithCPU("Intel Core i7 3770K")
                .WithRAM(8)
                .WithGPU("Intel HD Graphics 4000")
                .WithComponent("256GB SSD")
                .Build();
        }
    }

    public class ProComputerFactory : IComputerFactory
    {
        public Computer CreateComputer()
        {
            return new ComputerBuilder()
                .WithCPU("Intel Core 7 Ultra 290s")
                .WithRAM(64)
                .WithGPU("Nvidia RTX 5080")
                .WithComponent("cool keyboard")
                .WithComponent("xbox controller")
                .Build();
        }
    }

    public class MacMiniComputerFactory : IComputerFactory
    {
        public Computer CreateComputer()
        {
            return new ComputerBuilder()
                .WithCPU("Apple M2 PRO 10 Core CPU")
                .WithRAM(24)
                .WithGPU("Apple M2 PRO 10 Core GPU")
                .WithComponent("512GB SSD")
                .Build();
        }
    }


}
