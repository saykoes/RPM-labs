using GofPBS;

internal class Program
{
    private static void Main(string[] args)
    {
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