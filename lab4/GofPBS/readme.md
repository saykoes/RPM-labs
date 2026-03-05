## Lab 4. GOF: Builder, Prototype, Singleton
## Порождающие паттерны проектирования. Строитель, Одиночка, Прототип
### Цель работы: 
Изучить порождающие паттерны проектирования «Банды Четырех (GoF)» на примере разработки приложения-конфигуратора компьютерных систем. Освоить на практике паттерны **Строитель** (Builder) для пошагового конструирования сложных объектов, **Прототип** (Prototype) для клонирования объектов с различной глубиной копирования и **Одиночка** (Singleton)  для обеспечения единственного экземпляра класса с потокобезопасностью.
### Задание:
Разработать консольное приложение, моделирующее конфигуратор компьютерных систем. Приложение должно позволять:
1. Создавать объекты класса Computer, содержащие компоненты: процессор (строка), объём памяти (число), видеокарта (строка), список дополнительных комплектующих (список строк).

2. Использовать паттерны «Строитель» и «Фабричный метод» для создания различных конфигураций: офисный ПК, игровой ПК, домашний ПК.

3. Использовать паттерн Prototype для клонирования существующих конфигураций. Продемонстрировать разницу между поверхностным и глубоким копированием.

4. Использовать паттерн Singleton для создания реестра прототипов (PrototypeRegistry), который хранит предопределённые конфигурации (прототипы) и предоставляет доступ к ним. Реестр должен быть потокобезопасным.

--- 

## Stage 1: Builder

### Theory

&nbsp;
![Builder Diagram](Builder.drawio.svg)
&nbsp;
Here we have 
- `ComputerBuilder` as an Abstract builder
- `IComputerFactory` factories as Concrete builders
- `Computer` as a Product

### Practice

I've created a `Computer` class

```csharp
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
}
```

A `ComputerBuilder` class

```csharp
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
        // and if we would run the method again, 
        // it would modify the original _computer which reference we returned here
        Computer finishedComputer = _computer;
        _computer = new Computer(); 
        return finishedComputer;
    }
}
```

And `IComputerFactory` factories that use builder to get configured `Computer` objects

```csharp
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
```

In `Program.cs`

```csharp
IComputerFactory officeFactory = new OfficeComputerFactory();
Computer officeComp = officeFactory.CreateComputer();
officeComp.Display();

IComputerFactory proFactory = new ProComputerFactory();
Computer proComp = proFactory.CreateComputer();
proComp.Display();

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
```

*Output*
```
=== Computer Config ===
CPU: AMD Ryzen 7 7800X3D
RAN: 32 GB
GPU: NVidia RTX 4070 SUPER 12GB
Add. Components: MSI Motherboard, 10G Ethernet PCIe 3.0 x8 card
=======================
```

### Summary

I've successfully implemented Builder pattern

---

## Stage 2: Prototype

### Theory

&nbsp;
![Prototype Diagram](Prototype.drawio.svg)
&nbsp;
Here we have
- officeComp & proComp as Prototypes
- officeComp2 & proComp2 as Concrete prototypes

### Practice

I could've implemented `Clone` method from `ICloneable` but instead I've added `ShallowCopy` and `DeepCopy` methods separately

```csharp
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
```

In `Program.cs`

```csharp
IComputerFactory officeFactory = new OfficeComputerFactory();
Computer officeComp = officeFactory.CreateComputer();
Computer officeComp2.CPU = "Intel Core i5 8400";
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
```

*Output*
```
=== Computer Config ===
CPU: Intel Core i7 3770K
RAN: 8 GB
GPU: Intel HD Graphics 4000
Add. Components: 256GB SSD, thing only for shallow comp2
=======================
=== Computer Config ===
CPU: Intel Core i5 8400
RAN: 8 GB
GPU: Intel HD Graphics 4000
Add. Components: 256GB SSD, thing only for shallow comp2
=======================
```
ShallowCopy only copies reference to the `AdditionalComponents` list (our added component for `officeComp2` are being displayed in `officeComp`)
```
=== Computer Config ===
CPU: Intel Core 7 Ultra 290s
RAN: 64 GB
GPU: Nvidia RTX 5080
Add. Components: cool keyboard, xbox controller
=======================
=== Computer Config ===
CPU: Intel Core 7 Ultra 290s
RAN: 64 GB
GPU: Nvidia RTX 5080
Add. Components: cool keyboard, xbox controller, thing only for deep comp2
=======================
```
DeepCopy copies the whole `AdditionalComponents` list
### Summary
I've successfully implemented Prototype pattern and examined difference between `ShallowCopy` and `DeepCopy`

---

## Stage 3: Singleton

### Theory

&nbsp;
![Singleton Diagram](Singleton.drawio.svg)
&nbsp;
Here we have `PrototypeRegistry` as a Singleton that stores Dictionary of `Computer` objects and returns their deep copies

### Practice

I've added `PrototypeRegistry` class

I could've made it thread-safe by Double-Check Locking where we create `volatile` class and a `_lock`, that locks isntance creation after the first use  

But instead I've chosen to make it via static `Lazy<T>` that does this for us

```csharp
public sealed class PrototypeRegistry
{
    private static readonly Lazy<PrototypeRegistry> _instance =
        new Lazy<PrototypeRegistry>(() => new PrototypeRegistry());

    private readonly Dictionary<string, Computer> _dict;

    private PrototypeRegistry()
    {
        _dict = new Dictionary<string, Computer>
    {
        {"office", new OfficeComputerFactory().CreateComputer() },
        {"pro", new ProComputerFactory().CreateComputer() },
        {"mac", new MacMiniComputerFactory().CreateComputer() }
    };
    }

    public static PrototypeRegistry Instance => _instance.Value;

    public Computer GetPrototype(string key)
    {
        if (_dict.TryGetValue(key, out var prototype))
        {
            return prototype.DeepCopy();
        }
        return new Computer();
    }
}
```

In `Program.cs`

```csharp
PrototypeRegistry prot = PrototypeRegistry.Instance;
Computer officePc1 = prot.GetPrototype("office");
officePc1.RAM = 12;
officePc1.AdditionalComponents.Add("thing for only comp1 proto");
Computer officePc2 = prot.GetPrototype("office");

officePc1.Display();
officePc2.Display();
```

```csharp
PrototypeRegistry prot2 = PrototypeRegistry.Instance;
Console.WriteLine(ReferenceEquals(prot, prot2));
```

*Output*

```
=== Computer Config ===
CPU: Intel Core i7 3770K
RAN: 12 GB
GPU: Intel HD Graphics 4000
Add. Components: 256GB SSD, thing for only comp1 proto
=======================
=== Computer Config ===
CPU: Intel Core i7 3770K
RAN: 8 GB
GPU: Intel HD Graphics 4000
Add. Components: 256GB SSD
=======================
```

```
True
```

`True` indicates that `prot` and `prot2` use the same `PrototypeRegistry` instance: even if we try to create new one we are just getting reference to the original instance

### Summary

I've successfully implemented thread-safe Singleton pattern
