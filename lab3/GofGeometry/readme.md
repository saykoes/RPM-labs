## Lab 3
## Порождающие паттерны проектирования. Фабричный метод и абстрактная фабрика.
### Цель работы: 
Изучить часть порождающих паттернов проектирования «Банды Четырех (GoF)»
на примере последовательной эволюции архитектуры приложения: от использования
простого фабричного метода к более гибкому паттерну "Абстрактная фабрика". На
практике освоить принципы объектно-ориентированного проектирования, такие как
инкапсуляция создания объектов, слабая связанность и расширяемость.
### Задание:
Разработать оконное приложение с использованием фреймворка WPF, которое позволяет
пользователю создавать набор геометрических фигур: круг, квадрат, треугольник.
Цвет фигур выбирается из выпадающего списка (например, красный, синий, зелёный
или light, dark, colorful темы).
При изменении выбора все ранее созданные фигуры должны быть удалены и созданы
новые соответствующего цвета.

Работа выполняется в **два** этапа: 
1. Реализация с использованием паттерна "Фабричный метод".
Сначала проектируется решение, в котором для каждого типа фигуры создаётся своя иерархия классов-создателей.      

2. Рефакторинг до паттерна "Абстрактная фабрика".
Все фабричные методы группируются в один интерфейс абстрактной фабрики,
объявляющий методы создания всех трёх фигур. Затем создаются конкретные фабрики
для каждого цвета, реализующие все три метода. Клиентский код теперь работает
только с интерфейсом фабрики.

**ОБЯЗАТЕЛЬНО** в вашем репозитории должно быть 2 коммита
– с фабричным методом и абстрактной фабрикой соответственно.

---

## Part 1. Factory Method

### Theory

I've decided to draw a diagram of classes in the Project

&nbsp;
![Factory Method Diagram](FactoryMethod.drawio.svg)
&nbsp;

Here we can see that there are 3 different creators for each shape. Creators delegate implementation of creation to concrete creators (of different colors) 

### Practice
Created classes for figures (they are the same as in the example)

```csharp
public abstract class Figure
{
    public Color Color { get; set; }
    public abstract UIElement CreateUIElement(double size = 110);
}
```

```csharp
public class Circle : Figure
{
    public override UIElement CreateUIElement(double size = 105) => new Ellipse
        {
            Width = size,
            Height = size,
            Fill = new SolidColorBrush(Color),
            Margin = new Thickness(10)
        };
}
```

```csharp
public class Square : Figure
{
    public override UIElement CreateUIElement(double size = 100) => new Rectangle
        {
            Width = size,
            Height = size,
            Fill = new SolidColorBrush(Color),
            Margin = new Thickness(10)
        };
}
```

```csharp
public class Triangle : Figure
{
    public override UIElement CreateUIElement(double size = 100) => new Polygon
    {
        Points = new PointCollection
        {
            new Point(size / 2, 0),
            new Point(0, size),
            new Point(size, size)
        },
        Fill = new SolidColorBrush(Color),
        Margin = new Thickness(10),
        Width = size,
        Height = size,
        Stretch = Stretch.Fill
    };
}
```
Made `CircleCreator` but added more colors

```csharp
public abstract class CircleCreator
{
    public abstract Circle CreateCircle();
}
```
```csharp
public class RedCircleCreator : CircleCreator
{
    public override Circle CreateCircle() => new Circle { Color = Colors.Red };
}
public class GreenCircleCreator : CircleCreator
{
    public override Circle CreateCircle() => new Circle { Color = Colors.Green };
}
public class BlueCircleCreator : CircleCreator
{
    public override Circle CreateCircle() => new Circle { Color = Colors.Blue };
}
public class CyanCircleCreator : CircleCreator
{
    public override Circle CreateCircle() => new Circle { Color = Colors.Cyan };
}
public class MagentaCircleCreator : CircleCreator
{
    public override Circle CreateCircle() => new Circle { Color = Colors.Magenta };
}
public class YellowCircleCreator : CircleCreator
{
    public override Circle CreateCircle() => new Circle { Color = Colors.Yellow };
}
public class BlackCircleCreator : CircleCreator
{
    public override Circle CreateCircle() => new Circle { Color = Colors.Black };
}
```
Same with `SquareCreator`

```csharp
public abstract class SquareCreator
{
    public abstract Square CreateSquare();
}
```
```csharp
public class RedSquareCreator : SquareCreator
{
    public override Square CreateSquare() => new Square { Color = Colors.Red };
}
// ...
```
And `TriangleCreator`

```csharp
public abstract class TriangleCreator
{
    public abstract Triangle CreateTriangle();
}
```
```csharp
public class RedTriangleCreator : TriangleCreator
{
    public override Triangle CreateTriangle() => new Triangle { Color = Colors.Red };
}
// ...
```


**MainWindow (CS)**

Declared each creator
```csharp
private CircleCreator _currentCircleCreator;
private SquareCreator _currentSquareCreator;
private TriangleCreator _currentTriangleCreator;
```

Added `FiguresUpdate()` Method (changed switch condition from `Content.ToString()` to `SelectedIndex`)
```csharp
private void FiguresUpdate()
{
    switch (ColorComboBox.SelectedIndex)
    {
        // Red
        case 0:
            _currentCircleCreator = new RedCircleCreator();
            _currentSquareCreator = new RedSquareCreator();
            _currentTriangleCreator = new RedTriangleCreator();
            break;
        // Green
        case 1:
            _currentCircleCreator = new GreenCircleCreator();
            _currentSquareCreator = new GreenSquareCreator();
            _currentTriangleCreator = new GreenTriangleCreator();
            break;
        // ...
        default:
            return;
    }

    FiguresPanel.Children.Clear();

    FiguresPanel.Children.Add(_currentCircleCreator.CreateCircle().CreateUIElement());
    FiguresPanel.Children.Add(_currentSquareCreator.CreateSquare().CreateUIElement());
    FiguresPanel.Children.Add(_currentTriangleCreator.CreateTriangle().CreateUIElement());
}
```
Called `FiguresUpdate()` on init
```csharp
public MainWindow()
{
    InitializeComponent();
    FiguresUpdate();
}
```
...and on `ComboBox_SelectionChanged`
```csharp
private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
{
    FiguresUpdate();
}
```

**MainWindow (XAML)**

Nothing really interesting
```xml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="*"/>
        <RowDefinition Height="50px"/>
    </Grid.RowDefinitions>

    <StackPanel Grid.Row="0" x:Name="FiguresPanel" Orientation="Horizontal" HorizontalAlignment="Center"/>

    <StackPanel Grid.Row="1" Orientation="Horizontal" Margin="10" HorizontalAlignment="Center">
        <Label Content="Color:" VerticalAlignment="Center" />
        <ComboBox x:Name="ColorComboBox" Width="120" SelectionChanged="ComboBox_SelectionChanged" 
            VerticalAlignment="Center" HorizontalContentAlignment="Center">
            <ComboBoxItem Content="Red" IsSelected="True"/>
            <ComboBoxItem Content="Green"/>
            <ComboBoxItem Content="Blue"/>
            <ComboBoxItem Content="Cyan"/>
            <ComboBoxItem Content="Magenta"/>
            <ComboBoxItem Content="Yellow"/>
            <ComboBoxItem Content="blacK"/>
        </ComboBox>
    </StackPanel>
</Grid>
```

### Summary

You can add new colors and shapes without touching the original code but if you want to:
- **Add new color**
    Then, you need to implement it for all other shapes as well (and you have to check that manually!)
    
- **Add new shape**
    Then, you need to implement all of exisitng colors in shapes (which again, you have to check for manually)

Also, there is a ton of repetitive code... Yuck!

---

## Part 2. Abstract Factory

### Theory

&nbsp;
![Abstract Factory Method Diagram](AbstractFactoryMethod.drawio.svg)
&nbsp;
Instead of figure factory we now have factories for each color and an `IGeoFactory` interface that requires each color factory to implement every figure
### Practice

`IGeoFactory` interface
```csharp
internal interface IGeoFactory
{
    Circle CreateCircle();
    Square CreateSquare();
    Triangle CreateTriangle();
}
```
Factories with `IGeoFactory` implementation
```csharp
public class RedFactory : IGeoFactory
{
    public Circle CreateCircle() => new Circle { Color = Colors.Red };
    public Square CreateSquare() => new Square { Color = Colors.Red };
    public Triangle CreateTriangle() => new Triangle { Color = Colors.Red };
    
}
public class GreenFactory : IGeoFactory
{
    public Circle CreateCircle() => new Circle { Color = Colors.Green };
    public Square CreateSquare() => new Square { Color = Colors.Green };
    public Triangle CreateTriangle() => new Triangle { Color = Colors.Green };
}
// ...
```
**MainWindow (CS)**

Now we have one `_currentFactory` for all shapes
```csharp
private IGeoFactory _currentFactory;
```
And `UpdateFigures()` that now works with `_currentFactory`
```csharp
private void FiguresUpdate()
{
    switch (ColorComboBox.SelectedIndex)
    {
        // Red
        case 0:
            _currentFactory = new RedFactory();
            break;
        // Green
        case 1:
            _currentFactory = new GreenFactory();
            break;
        // ...
            break;
        default:
            return;
    }

    FiguresPanel.Children.Clear();

    FiguresPanel.Children.Add(_currentFactory.CreateCircle().CreateUIElement());
    FiguresPanel.Children.Add(_currentFactory.CreateSquare().CreateUIElement());
    FiguresPanel.Children.Add(_currentFactory.CreateTriangle().CreateUIElement());
}
```

### Summary
Now to add new color we just need to add new class that implements `IGeoFactory` interface
If we need to add new shape, we stiil would need to change the code, but at least now compiler would thow an exception if you forget to implement new shape in some color

Code is now less repetitive

---

## Bonus: MVVM, SOLID and App Independant code
### Theory
In FiguresUpdate() there is this long switch statement
```csharpharp
switch (ColorComboBox.SelectedIndex)
{
    // Red
    case 0:
        _currentFactory = new RedFactory();
        break;
    // Green
    case 1:
        _currentFactory = new GreenFactory();
        break;
    // ... other colors ...
    default:
        return;
}
```

I don't like it. In order to add new colors you have to modify the UI code (which can be ignored since it isn't the backend)

But then, I remembered how I did menu selection in [my previous project using Avalonia.UI](https://github.com/saykoes/JPDA/blob/main/JPDA/ViewModels/MainViewModel.cs)
Let's see **that project's code** briefly to get the underlying idea

Instead of using `ListItem` in the `ListBox` and then checking selected `ListItem` to know what ViewModel to call, let's make `ListBox` use our array of our own type/class directly and just create ViewModel instance of the selected `ListItem`

I've declared new class for item in the list (to include name,icon and viewmodel)
```csharp
public class ListItemTemplate(Type type, string? title, StreamGeometry? icon)
{
    public string? Label { get; set; } = title;
    public Type ModelType { get; set; } = type;
    public StreamGeometry? ListItemIcon { get; } = icon;
}
```
Declared `ObservableCollection` (to be used as a `ItemSource` in `ListBox` in the View) 
```csharp
public static ObservableCollection<ListItemTemplate> MenuItems { get; } =
[
    new ListItemTemplate(typeof(KanjiViewModel), GetUiString("ui_menu_kanji_input"), GetIcon("EditRegular")),
    new ListItemTemplate(typeof(DictionaryViewModel), GetUiString("ui_menu_dictionary"), GetIcon("BookRegular")),
    new ListItemTemplate(typeof(SettingsViewModel), GetUiString("ui_settings"), GetIcon("SettingsRegular"))
];
```
Declared item that is selected in the `ListBox`
```csharp
[ObservableProperty] 
private static ListItemTemplate? _selectedListItem;
```
Redefined automatically generated method (from`[ObservableProperty]` from `CommunityToolkit.MVVM`) to create instance of ViewModel of selected item 
```csharp
partial void OnSelectedListItemChanged(ListItemTemplate? value)
{
    if (value is null) return;
    var instance = Activator.CreateInstance(value.ModelType);
    if (instance is null) return;
    CurrentPage = (ViewModelBase)instance;
    IsMenuPaneOpen = false;
}
```
### Practice
Let's get back to **this project**

Now, we have a very distinct difference in project structure: We don't have a base class for all factories. 
I could change the code, but I didn't want to touch the backend at all. Moreover, explicit interface contracts are much more readable and flexible than implicit inheritance from base class

Instead of using `ComboBoxItem` in the `ComboBox` and then checking selected `ComboBoxItem.SelectedIndex` to know what factory to call, let's make `ComboBox` use our array of our own type/class directly and just call factory instance of the selected `ComboBoxItem`

I've switched to MVVM. It will increase code independance from the app and also make everything "in one place"

I've declared `ColorOption` `record` instead of class (like in that project's code) (record provieds less code, immutability, etc.)
```csharp
public record ColorOption(IGeoFactory Factory, string Label);
```
Now we have a problem. To put our factory in `ColorOption` we need to create an instance of it

Let's use reflection, check what factories we have in the code and create and array of factory instances
I've declared separate class for providing factories
```csharp
internal class GeoFactoryProvider
{
    private static readonly List<IGeoFactory> _instances;
    static GeoFactoryProvider()
    {
        _instances = typeof(IGeoFactory).Assembly.GetTypes()                                       
            .Where(t => typeof(IGeoFactory).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract) 
            .Select(t => (IGeoFactory)Activator.CreateInstance(t)!)                                
            .ToList();
    }
    public static IEnumerable<IGeoFactory> GetFactories() => _instances;
}
```
Declared selected item color as an `[ObservableProperty]` and used the trick from previous project
```csharp
[ObservableProperty]
private ColorOption? _selectedColor;
```
```csharp
partial void OnSelectedColorChanged(ColorOption? value) => UpdateFigures();
```
Used Constructor Chaining to so that the View doesn't have to include arguments to call the ViewModel
```csharp
public MainViewModel() : this(GeoFactoryProvider.GetFactories()) { }
```
Let's create our `ObservableCollection` for ItemSource. Used reflection to get names for colors
```csharp
public MainViewModel(IEnumerable<IGeoFactory> factories)
{
    var options = factories.Select(f => new ColorOption(f, f.GetType().Name.Replace("Factory", "")));
    ColorOptions = new ObservableCollection<ColorOption>(options);
    SelectedColor = ColorOptions.FirstOrDefault();
}
```
And let's create figures with `SelectedItem`'s factory
```csharp
public ObservableCollection<UIElement> Figures { get; } = new();
```
```csharp
private void UpdateFigures()
{
    Figures.Clear();
    if (SelectedColor == null || SelectedColor.Factory == null) return;
    var factory = SelectedColor.Factory;

    Figures.Add(factory.CreateCircle().CreateUIElement());
    Figures.Add(factory.CreateSquare().CreateUIElement());
    Figures.Add(factory.CreateTriangle().CreateUIElement());
}
```
**MainWindow (XAML)**

Let's make ViewModel our DataContext
```xml
<Window.DataContext>
    <local:MainViewModel />
</Window.DataContext>
```
Switch to ItemsControl to have ItemSource from the ViewModel
```xml    
<ItemsControl Grid.Row="0" ItemsSource="{Binding Figures}">
    <ItemsControl.ItemsPanel>
        <ItemsPanelTemplate>
            <StackPanel Orientation="Horizontal" HorizontalAlignment="Center"/>
        </ItemsPanelTemplate>
    </ItemsControl.ItemsPanel>
</ItemsControl>
```
Set ItemSource and SelectedItem in ComboBox color selector
```xml
<StackPanel Grid.Row="1" Orientation="Horizontal" Margin="10" HorizontalAlignment="Center">
    <Label Content="Color:" VerticalAlignment="Center" />
    <ComboBox Width="120" 
            VerticalAlignment="Center" 
            HorizontalContentAlignment="Center"
            ItemsSource="{Binding ColorOptions}"
            SelectedItem="{Binding SelectedColor}"
            DisplayMemberPath="Label" />
</StackPanel>
```

### Result
And now we are able to add new colors by just declaring new factory without ever touching even the UI code (all what's needed is an IGeoFactory interaface implementation)