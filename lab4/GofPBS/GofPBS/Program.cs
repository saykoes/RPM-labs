using GofPBS;

internal class Program
{
    private static void Main(string[] args)
    {
        IComputerFactory officeFactory = new OfficeComputerFactory();
        Computer officeComp = officeFactory.CreateComputer();

        Computer officeComp2 = officeComp.ShallowCopy();
        officeComp2.AdditionalComponents.Add("thing only for shallow comp2");

        officeComp.Display();
        officeComp2.Display();

        IComputerFactory proFactory = new ProComputerFactory();
        Computer proComp = proFactory.CreateComputer();
        
        Computer proComp2 = proComp.DeepCopy();
        proComp2.AdditionalComponents.Add("thing only for deep comp2");

        proComp.Display();
        proComp2.Display();

        IComputerFactory macFactory = new MacMiniComputerFactory();
        Computer macComp = macFactory.CreateComputer();
        macComp.Display();

        Computer customComputer = new ComputerBuilder()
        .WithCPU("AMD Ryzen 7 7800X3D")
        .WithRAM(32)
        .WithGPU("NVidia RTX 4070 SUPER 12GB")
        .WithComponent("MSI Motherboard")
        .WithComponent("10G Ethernet PCIe 3.0 x8 card")
        .Build();



         customComputer.Display();
    }
}