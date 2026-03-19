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

`CharacterFactory` class
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
What if we want to have Lazy initialization? We can use Proxy pattern to initialize proxy object first and then load an actual object, once one of proxy's method is called

### Practice
I created single interface for Images and Proxies of Images so that we can work with both seamlessly
```csharp
public interface IImage : IRenderable
{
    int GetWidth();
    int GetHeight();
}
```
Created a class for an actual `HighResolutionImage`
```csharp
 public class HighResolutionImage : IImage
 {
     private string _filename;
     private int _width;
     private int _height;
     public HighResolutionImage(string filename)
     {
         _filename = filename;
         Console.Write($"Loading {_filename}... ");
         LoadFromDisk();
     }
     private void LoadFromDisk()
     {
         // Slow loading imitation
         Thread.Sleep(1000);
         _width = 1920;
         _height = 1080;
         Console.WriteLine($"loaded ({_width}x{_height})");
     }
     public void Render(int posX, int posY)
     {
         Console.WriteLine($"Rendering image '{_filename}' at ({posX},{posY})");
     }
     public int GetWidth() => _width;
     public int GetHeight() => _height;
 }
```
And now it's time to create `Proxy`
```csharp
 public class ImageProxy : IImage
 {
     private string _filename;
     private HighResolutionImage? _realImage; // nullable
     public ImageProxy(string filename)
     {
         _filename = filename;
         Console.WriteLine($"[Proxy] Craeted proxy for {_filename}");
     }
     private void EnsureLoaded()
     {
         if (_realImage == null)
         {
             Console.WriteLine($"[Proxy] Method is called: Loading object");
             _realImage = new HighResolutionImage(_filename);
         }
     }
     public void Render(int posX, int posY)
     {
         EnsureLoaded();
         Console.Write($"[Proxy]");
         _realImage?.Render(posX, posY);
     }
     public int GetWidth()
     {
         EnsureLoaded();
         Console.WriteLine($"[Proxy]");
         return _realImage?.GetWidth() ?? 0;
     }
     public int GetHeight()
     {
         EnsureLoaded();
         Console.WriteLine($"[Proxy]");
         return _realImage?.GetHeight() ?? 0;
     }
 }
```
Because of `EnsureLoaded` method, image wouldn't be null, but I added null checks in other methods just in case
### Program.cs (Main)

```csharp
// Proxy

Console.WriteLine();

// (will not load imgs)
IImage img1 = new ImageProxy("image1.png");
IImage img2 = new ImageProxy("image2.png");
IImage img3 = new ImageProxy("image3.png");

Console.WriteLine();

Console.WriteLine($"img1 Width: {img1.GetWidth()}px"); // will load img1
img2.Render(34,56); // will load img2
Console.WriteLine($"img1 Heigth: {img1.GetHeight()}px"); // already loaded

// img3 won't be loaded
```

*Output*
```
[Proxy] Craeted proxy for image1.png
[Proxy] Craeted proxy for image2.png
[Proxy] Craeted proxy for image3.png

[Proxy] Method is called: Loading object
Loading image1.png... loaded (1920x1080)
[Proxy]
img1 Width: 1920px
[Proxy] Method is called: Loading object
Loading image2.png... loaded (1920x1080)
[Proxy]Rendering image 'image2.png' at (34,56)
[Proxy]
img1 Heigth: 1080px
```
img3 wasn't loaded indeed
### Summary
I've successfully implemented Proxy pattern

---

## Step 3: Bridge

### Theory

But what if I want to separate Implimentaion (rendering in our case) from Abstraction (shapes, characters etc.)? 
I can use Bridge pattern for that

### Practice

### Abstraction (Shapes)
I've created interfaces for shapes

```csharp
public interface IDrawable
{
    void Draw();
}
public abstract class GraphicObject : IDrawable
{
    protected IRenderingEngine _engine;
    public GraphicObject(IRenderingEngine engine)
    {
        _engine = engine;
    }
    public abstract void Draw();
    public abstract void Move(float dx, float dy);
}
```

Rectangle
```csharp
internal class Rectangle : GraphicObject
{
    private float _x, _y, _width, _height;
    public Rectangle(IRenderingEngine engine, float x, float y, float width, float height) : base(engine)
    {
        _x = x;
        _y = y;
        _width = width;
        _height = height;
    }
    public override void Draw()
    {
        _engine.RenderRectangle(_x, _y, _width, _height);
    }
    public override void Move(float dx, float dy)
    {
        _x += dx;
        _y += dy;
        Console.WriteLine($"[Rectangle] Moved by ({dx};{dy}), now it's at: ({_x};{_y})");
    }
}
```

Ellipse
```csharp
internal class Ellipse : GraphicObject
{
    private float _x, _y, _radiusX, _radiusY;
    public Ellipse(IRenderingEngine engine, float x, float y, float radiusX, float radiusY) : base(engine)
    {
        _x = x;
        _y = y;
        _radiusX = radiusX;
        _radiusY = radiusY;
    }
    public override void Draw()
    {
        _engine.RenderEllipse(_x, _y, _radiusX, _radiusY);
    }
    public override void Move(float dx, float dy)
    {
        _x += dx;
        _y += dy;
        Console.WriteLine($"[Ellipse] Moved by ({dx};{dy}), now it's at: ({_x};{_y})");
    }
}
```

And Line
```csharp
internal class Line : GraphicObject
{
    private float _x1, _y1, _x2, _y2;
    public Line(IRenderingEngine engine, float x1, float y1, float x2, float y2) : base(engine)
    {
        _x1 = x1; _y1 = y1;
        _x2 = x2; _y2 = y2;
    }
    public override void Draw()
    {
        _engine.RenderLine(_x1, _y1, _x2, _y2);
    }
    public override void Move(float dx, float dy)
    {
        _x1 += dx; _y1 += dy;
        _x2 += dx; _y2 += dy;
        Console.WriteLine($"[Line] Moved by ({dx};{dy}), now it's at: ({_x1};{_y1})--({_x2};{_y2})");
    }
}
```

### Implementation (Rendering)

Created single interface for renderers
```csharp
public interface IRenderingEngine
{
    void BeginRender();
    void EndRender();
    void RenderRectangle(float x, float y, float width, float height);
    void RenderEllipse(float x, float y, float radiusX, float radiusY);
    void RenderLine(float x1, float y1, float x2, float y2);
}
```

Added first type of renderer
```csharp
internal class PrintRenderer : IRenderingEngine
{
    public void BeginRender() =>
        Console.WriteLine("[Print] Render Start");
    public void EndRender() =>
        Console.WriteLine("[Print] Render End");
    public void RenderRectangle(float x, float y, float width, float height) =>
        Console.WriteLine($"[Print] Rectangle ({x};{y}) {width}x{height}");
    public void RenderEllipse(float x, float y, float radiusX, float radiusY) =>
        Console.WriteLine($"[Print] Ellipse ({x};{y}) radius (rx: {radiusX}; ry:{radiusY})");
    public void RenderLine(float x1, float y1, float x2, float y2) =>
        Console.WriteLine($"[Print] Line ({x1};{y1})--({x2};{y2})");
}
```

And the second one

```csharp
public class ScreenRenderer : IRenderingEngine
{
    public void BeginRender() =>
        Console.WriteLine("[Screen] Render Start");
    public void EndRender() =>
        Console.WriteLine("[Screen] Render End");
    public void RenderRectangle(float x, float y, float width, float height) =>
        Console.WriteLine($"[Screen] Rectangle ({x};{y}) {width}x{height}");
    public void RenderEllipse(float x, float y, float radiusX, float radiusY) =>
        Console.WriteLine($"[Screen] Ellipse ({x};{y}) radius (rx: {radiusX},ry:{radiusY})");
    public void RenderLine(float x1, float y1, float x2, float y2) =>
        Console.WriteLine($"[Screen] Line ({x1};{y1})--({x2};{y2})");
}
```

### Program.cs

```csharp
// Bridge

Console.WriteLine();

IRenderingEngine screenEngine = new ScreenRenderer();
IRenderingEngine printEngine = new PrintRenderer();

// printEngine
Rectangle printRect = new Rectangle(printEngine, 15.5f, 30.0f, 120.0f, 60.5f);
Ellipse printEllipse = new Ellipse(printEngine, 250.0f, 150.0f, 45.0f, 45.0f);
Line printLine = new Line(printEngine, 10.0f, 10.0f, 300.0f, 450.0f);

printEngine.BeginRender();
printRect.Draw();
printEllipse.Draw();
printLine.Draw();
printEngine.EndRender();

Console.WriteLine();

// screenEngine
Rectangle screenRect = new Rectangle(screenEngine, 15.5f, 30.0f, 120.0f, 60.5f);
Ellipse screenEllipse = new Ellipse(screenEngine, 250.0f, 150.0f, 45.0f, 45.0f);
Line screenLine = new Line(screenEngine, 10.0f, 10.0f, 300.0f, 450.0f);

screenEngine.BeginRender();
screenRect.Draw();
screenEllipse.Draw();
screenLine.Draw();
screenEngine.EndRender();

Console.WriteLine();

printLine.Move(5.0f, 10.0f);
screenEngine.BeginRender();
printLine.Draw();
screenEngine.EndRender();
```

*Output*
```
[Print] Render Start
[Print] Rectangle (15.5;30) 120x60.5
[Print] Ellipse (250;150) radius (rx: 45; ry:45)
[Print] Line (10;10)--(300;450)
[Print] Render End

[Screen] Render Start
[Screen] Rectangle (15.5;30) 120x60.5
[Screen] Ellipse (250;150) radius (rx: 45; ry:45)
[Screen] Line (10;10)--(300;450)
[Screen] Render End

[Line] Moved by (5;10), now it's at: (15;20)--(305;460)
[Screen] Render Start
[Screen] Line (15;20)--(305;460)
[Screen] Render End
```

### Summary

I've successfully implemented Bridge pattern

---

## Step 4: Decorator

### Theory

What if I want to add visual effects for each individual object separately on runtime, without adding a lot of classes for each edge case? I can use Decorator for that. Decorator will store a link to the original object and will apply its own effects on top of it

### Practice

Let's create a base class for all of Decorators

```csharp
public abstract class DrawableDecorator : IDrawable
{
    protected IDrawable _wrappee; // original object
    public DrawableDecorator(IDrawable wrappee)
    {
        _wrappee = wrappee;
    }
    public virtual void Draw()
    {
        _wrappee.Draw(); // draw the original object
    }
}
```

Border decorator
```csharp
internal class BorderDecorator : DrawableDecorator
{
    private int _borderWidth;
    public BorderDecorator(IDrawable wrappee, int borderWidth) : base(wrappee)
    {
        _borderWidth = borderWidth;
    }
    public override void Draw()
    {
        base.Draw(); // draw the original object
        Console.WriteLine($"|---[Border (Width: {_borderWidth}px)]"); // draw effect on top of it
    }
}
```

Shadow decorator
```csharp
internal class ShadowDecorator : DrawableDecorator
{
    private int _shadowOffset;
    public ShadowDecorator(IDrawable wrappee, int shadowOffset) : base(wrappee)
    {
        _shadowOffset = shadowOffset;
    }
    public override void Draw()
    {
        base.Draw();
        Console.WriteLine($"|---[Shadow (Offset: {_shadowOffset}px)]");
    }
}
```

Transparency decorator
```csharp
internal class TransparencyDecorator : DrawableDecorator
{
    private int _transparencyLevel;
    public TransparencyDecorator(IDrawable wrappee, int transparencyLevel) : base(wrappee)
    {
        _transparencyLevel = transparencyLevel;
    }
    public override void Draw()
    {
        base.Draw();
        Console.WriteLine($"|---[Transpareny ({_transparencyLevel}%)]");
    }
}
```

And let's create `Document` class, where we'll be able to store `Pages` in which there'll be `IDrawable` objects

```csharp
public class Document(IRenderingEngine engine)
{
    private List<Page> _pages = new List<Page>();
    public Page CreatePage()
    {
        var page = new Page();
        _pages.Add(page);
        return page;
    }
    public void RenderAll()
    {
        engine.BeginRender();
        for (int i = 0; i < _pages.Count; i++)
        {
            Console.WriteLine($"\n--- Page {i + 1} ---");
            _pages[i].Render();
        }
        engine.EndRender();
    }
}
```
```csharp
public class Page
{
    private List<IDrawable> _drawables = new List<IDrawable>();
    public void Add(IDrawable drawable)
    {
        _drawables.Add(drawable);
    }
    public void Render()
    {
        foreach (var d in _drawables)
        {
            d.Draw();
            Console.WriteLine();
        }
    }
}
```

### Program.cs

```csharp
Rectangle screenRect = new Rectangle(screenEngine, 15.5f, 30.0f, 120.0f, 60.5f);
Ellipse screenEllipse = new Ellipse(screenEngine, 250.0f, 150.0f, 45.0f, 45.0f);
Line screenLine = new Line(screenEngine, 10.0f, 10.0f, 300.0f, 450.0f);
// .... //
IDrawable neonRect = new ShadowDecorator(screenRect, 12);
IDrawable thickBorderEllipse = new BorderDecorator(screenEllipse, 8);
IDrawable ghostLine = new TransparencyDecorator(screenLine, 85);

neonRect.Draw(); Console.WriteLine();
thickBorderEllipse.Draw(); Console.WriteLine();
ghostLine.Draw(); Console.WriteLine();
```
```
-- *Output* --

[Screen] Rectangle (15.5;30) 120x60.5
|---[Shadow (Offset: 12px)]

[Screen] Ellipse (250;150) radius (rx: 45,ry:45)
|---[Border (Width: 8px)]

[Screen] Line (10;10)--(300;450)
|---[Transpareny (85%)]
```

```csharp
IDrawable glassRect = new BorderDecorator(
                        new TransparencyDecorator(screenRect, 40), 2);

IDrawable heavyEllipse = new BorderDecorator(
                            new ShadowDecorator(screenEllipse, 15), 4);

IDrawable complexLine = new BorderDecorator(
                            new TransparencyDecorator(
                                new ShadowDecorator(screenLine, 6), 25), 1);

glassRect.Draw(); Console.WriteLine();
heavyEllipse.Draw(); Console.WriteLine();
complexLine.Draw(); Console.WriteLine();
```
```
-- *Output* --

[Screen] Rectangle (15.5;30) 120x60.5
|---[Transpareny (40%)]
|---[Border (Width: 2px)]

[Screen] Ellipse (250;150) radius (rx: 45,ry:45)
|---[Shadow (Offset: 15px)]
|---[Border (Width: 4px)]

[Screen] Line (10;10)--(300;450)
|---[Shadow (Offset: 6px)]
|---[Transpareny (25%)]
|---[Border (Width: 1px)]
```

```csharp
Document mainDoc = new Document(screenEngine);

Page coverPage = mainDoc.CreatePage(); 
coverPage.Add(screenEllipse);
coverPage.Add(neonRect);
coverPage.Add(heavyEllipse);

Page contentPage = mainDoc.CreatePage();
contentPage.Add(screenLine);
contentPage.Add(ghostLine);
contentPage.Add(glassRect);
contentPage.Add(complexLine);

mainDoc.RenderAll();
```
```
-- *Output* --

[Screen] Render Start

--- Page 1 ---
[Screen] Ellipse (250;150) radius (rx: 45,ry:45)

[Screen] Rectangle (15.5;30) 120x60.5
|---[Shadow (Offset: 12px)]

[Screen] Ellipse (250;150) radius (rx: 45,ry:45)
|---[Shadow (Offset: 15px)]
|---[Border (Width: 4px)]


--- Page 2 ---
[Screen] Line (10;10)--(300;450)

[Screen] Line (10;10)--(300;450)
|---[Transpareny (85%)]

[Screen] Rectangle (15.5;30) 120x60.5
|---[Transpareny (40%)]
|---[Border (Width: 2px)]

[Screen] Line (10;10)--(300;450)
|---[Shadow (Offset: 6px)]
|---[Transpareny (25%)]
|---[Border (Width: 1px)]

[Screen] Render End
```

### Summary

I've successfully implemented Decorator pattern