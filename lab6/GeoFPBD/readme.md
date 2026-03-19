## Lab 6. GOF: Flywheight, Proxy, Bridge, Decorator
## Структурные паттерны проектирования. Приспособленец, Мост, Декоратор, Заместитель.
### Цель работы: 
Изучить структурные паттерны
проектирования GoF на примере разработки приложения для работы с графическими
документами и их визуализацией. Освоить на практике паттерны **Proxy** для
управления доступом к ресурсоёмким объектам, **Flyweight** для эффективного
разделения больших количеств мелких объектов, **Bridge** для разделения
абстракции и реализации, и **Decorator** для динамического добавления
обязанностей объектам.
### Задание:
Разработать консольное приложение, моделирующее систему
рендеринга сложных документов, содержащих графические примитивы (фигуры,
изображения, текст). 

Приложение должно позволять:
1. Эффективно работать с большим количеством повторяющихся объектов (символы, стили) через паттерн **Flyweight**, разделяя общее внутреннее состояние.

2. Организовать отложенную загрузку и кэширование ресурсоёмких объектов (изображения высокого разрешения) с помощью паттерна **Proxy**.

3. Реализовать независимое изменение иерархии фигур и способов их отрисовки (2D, 3D, векторная графика)
через паттерн **Bridge**.

4. Динамически добавлять функциональность (рамка, тень, прозрачность) к графическим объектам без изменения их кода с помощью паттерна **Decorator**.

--- 

## Step 1: Flyweight

### Theory

Flyweight pattern is needed when we want to save on RAM 

There are:
- `Flyweight`: provides interface 
- `ConcreteFlyweight`: shared object class that implements flyweight, Intrinsic state will be stored there 
- `UnsharedConcreteFlyweight`: non-shared object class that implements flyweight, Intrinsic state will be stored there 
- `FlyweightFactory`: creates both shared and non-shared objects, if shared, then checks if that object is already allocated in memory and uses that instead of creating new objects everytime
- `Client`: uses `FlyweightFactory` to get `Flyweight` objects, Extrinsic state will be stored there
### Practice

I've implemented `IRenderable` class
```csharp
// Flyweight 
public interface IRenderable
{
    void Render(int line, int col);
}
```

`Character` class
```csharp
// ConcreteFlyweight (Intrinsic state will be stored with it)
internal class Character(char symbol, string font, int fontSize) : IRenderable
{
    public void Render(int positionX, int positionY)
    {
        Console.WriteLine($"Rendered '{symbol}'({font}, {fontSize}pt)\tat ({positionX}, {positionY})");
    }
}
```
Using Primary constructor parameters here instead of fields

`ColorRender` class
```csharp
// UnsharedConcreteFlyweight (Intrinsic state will be stored with it)
public class ColorRender(string name, ConsoleColor color) : IRenderable
{
    public void Render(int line, int column)
    {
        Console.ForegroundColor = color;
        Console.WriteLine($"Custom Render '{name}'\tat ({line}, {column})");
        Console.ResetColor();
    }
}
```

`Something` class
```csharp
// FlyweightFactory 
internal class CharacterFactory
{
    private Dictionary<string, Character> _characters = new(); // Storing all created shared characters
    private int _uniqueChars = 0; // Counting unique instances
    public Character GetCharacter(char symbol, string font, int fontSize)
    {
        string key = $"{symbol}_{font}_{fontSize}"; // forming key for checking
        if (!_characters.ContainsKey(key))
        {
            _uniqueChars++;
            // create them and add to dictionary
            Console.WriteLine($"Initializing \t'{symbol}' ({font}, {fontSize}pt)");
            _characters[key] = new Character(symbol, font, fontSize);
        }
        else
        {
            // but if already created, use character from dictionary  
            Console.WriteLine($"Reusing \t'{symbol}' ({font}, {fontSize}pt)");
        }
        return _characters[key];
    }
    // overloading method for non-shared object
    public ColorRender GetCharacter(string name, ConsoleColor color)
    {
        _uniqueChars++;
        Console.WriteLine($"Initializing \t'{name}' ({color} color pt)");
        return new ColorRender(name, color);
    }
    public int GetCount() => _characters.Count;
}
```
### In Program.cs (Client)
Made `record` for characters with position (Extrinsic state will be stored with it)
```csharp
public record posChar (int posX, int posY, IRenderable CharObj);
```


```csharp
private static void Main(string[] args)
{ 
    CharacterFactory charFactory = new CharacterFactory();

    var screenChars = new List<posChar>
    {
        new posChar(5, 5, charFactory.GetCharacter('T', "Verdana", 16)),
        new posChar(100, 100, charFactory.GetCharacter('D', "Courier New", 14)),
        new posChar(50, 10, charFactory.GetCharacter('W', "Georgia", 18)),
        new posChar(15, 5, charFactory.GetCharacter('T', "Verdana", 16)),  // Same as first char
        new posChar(0, 200, charFactory.GetCharacter('P', "Impact", 20)),
        new posChar(110, 100, charFactory.GetCharacter('D', "Courier New", 14)), // Same as second char
        new posChar(11, 11, charFactory.GetCharacter("Color is cool!", ConsoleColor.Green)), // Custom
    };

    Console.WriteLine();

    foreach (var curChar in screenChars) 
        curChar.CharObj.Render(curChar.posX,curChar.posY);

    Console.WriteLine();

    Console.WriteLine($"Unique characters: {charFactory.GetCount()}");
}
```

*Output*
```
Initializing    'T' (Verdana, 16pt)
Initializing    'D' (Courier New, 14pt)
Initializing    'W' (Georgia, 18pt)
Reusing         'T' (Verdana, 16pt)
Initializing    'P' (Impact, 20pt)
Reusing         'D' (Courier New, 14pt)
Initializing    'Color is cool!' (Green color pt)

Rendered 'T'(Verdana, 16pt)     at (5, 5)
Rendered 'D'(Courier New, 14pt) at (100, 100)
Rendered 'W'(Georgia, 18pt)     at (50, 10)
Rendered 'T'(Verdana, 16pt)     at (15, 5)
Rendered 'P'(Impact, 20pt)      at (0, 200)
Rendered 'D'(Courier New, 14pt) at (110, 100)
Custom Render 'Color is cool!'  at (11, 11)
```
```
Unique characters: 5
```
Unique characters were allocated, while non-unique shared objects used a link to them
### Summary

I've successfully implemented Flyweight pattern

---

## Step 2: Proxy

### Theory

### Practice

### Program.cs

### Summary
I've successfully implemented Proxy pattern

---

## Step 3: Bridge

### Theory

### Practice

### Program.cs

### Summary

I've successfully implemented Bridge pattern

---

## Step 4: Decorator

### Theory

### Practice

### Program.cs

### Summary

I've successfully implemented Decorator pattern