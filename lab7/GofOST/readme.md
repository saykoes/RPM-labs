## Lab 7. GOF: Observer, Strategy, Template Method
## Поведенческие паттерны проектирования. Наблюдатель, Стратегия, Шаблонный метод
### Цель работы: 
Формирование практических навыков применения поведенческих
паттернов проектирования в рамках единой программной системы.
### Задание:
Необходимо разработать имитацию системы мониторинга и
оповещения о событиях, которая отслеживает различные метрики (загрузка
процессора, использование памяти, сетевая активность) и уведомляет подписчиков
о критических событиях. Система должна поддерживать различные каналы
уведомлений (консоль, файл) и форматы сообщений (текстовый, JSON, HTML).


Архитектура системы должна обеспечивать гибкость в
добавлении новых типов подписчиков, новых форматов сообщений и новых типов
обрабатываемых событий без существенной модификации существующего кода. Все три
паттерна должны работать согласованно, обеспечивая целостность системы и
возможность её масштабирования.

--- 

## Step 1: Observer

### Theory

What if we want to notify a lot of objects about our subject's changes? We can use Observrer pattern. There are two observer models:
- pull (subjects attach nothing to notification) and 
- push (subjects attach everything to notifications that theoretical object would need)

We're going to use push model, since that's the standart practice in C#

### Practice

I've created `MetricData` class
```csharp
public class MetricData(string metricName, double value, double threshold, DateTime timestamp)
{
    public string MetricName { get; } = metricName ?? throw new
    ArgumentNullException(nameof(metricName));
    public double Value { get; } = value;
    public double Threshold { get; } = threshold;
    public DateTime Timestamp { get; } = timestamp;
    public override string ToString() =>
        $"Metric: {MetricName}, Value: {Value} (Threshold: {Threshold})";
}
```

Then created `MetricEventArgs` that we're going to include in our Event
```csharp
public class MetricEventArgs(string eventType, MetricData data) : EventArgs
{
    public string EventType { get; } = eventType ?? 
        throw new ArgumentNullException(nameof(eventType));
    public MetricData Data { get; } = data ?? 
        throw new ArgumentNullException(nameof(data));
}
```

Delegate in which we're going to store our event
```csharp
public delegate void MetricEventHandler(MetricEventArgs e);
```

And `EventMonitor` that will orchestrate everything
```csharp
internal class EventMonitor
{
    // using Event and Invoke, instead of IObservable or something like that
    public event MetricEventHandler? OnMetricExceeded;
    public void CheckMetric(string metricName = "Temperature", double value = 110, double threshold = 100)
    {
        Console.WriteLine($"[Monitor]: '{metricName}' (Value: {value}; Threshold: {threshold})");
        if (value > threshold)
        {
            MetricData eventData = new MetricData(metricName, value, threshold, DateTime.Now);
            OnMetricExceeded?.Invoke(new MetricEventArgs(eventType: metricName +
            "_Exceeded", data: eventData)); // Invoke checks if there are any subscribers
        }
    }
}
```

### In Program.cs

```csharp
EventMonitor myMonitor = new EventMonitor();
myMonitor.OnMetricExceeded += e => Console.WriteLine($"{e.Data.MetricName} is too high!");
myMonitor.OnMetricExceeded += e => { if (e.Data.MetricName == "Temperature") Console.WriteLine("Throttling CPU down"); };
myMonitor.CheckMetric();
```

*Output*
```
[Monitor]: 'Temperature' (Value: 110; Threshold: 100)
Temperature is too high!
Throttling CPU down
```

### Summary

I've successfully implemented Observer pattern

---

## Step 2: Strategy

### Theory

What if we want to work with different implementations of one simillar thing via one common interface? We can use Strategy for that

### Practice
Let's create interface (Strategy)
```csharp
public interface IFormatStrategy
{
    string Format(string message, DateTime timestamp);
}
```
And its implementations (ConcreteStrateges)
```csharp
public class TextFormatStrategy : IFormatStrategy
{
    public string Format(string message, DateTime timestamp) =>
        $"[{timestamp:yyyy-MM-dd HH:mm:ss}] {message}";
}
```
```csharp
public class JsonFormatStrategy : IFormatStrategy
{
    public string Format(string message, DateTime timestamp) =>
        $"{{\"timestamp\":\"{timestamp:O}\",\"message\":\"{message}\"}}";
}
```

We're going to create context (which works with ConcreteStrategies via Startegy interface) in the next step

### Summary
I've successfully implemented Strategy pattern

---

## Step 3: Template Method

### Theory

What if we want to incapsulate simmilar logic of different objects in a base class and make specific logic in its children? We can utilize Template Method pattern.


### Practice

Let's make base class that will store basic logic
```csharp
public abstract class EventHandlerBase
{
    protected IFormatStrategy _formatStrategy; //текущая стратегия
    protected EventHandlerBase(IFormatStrategy strategy)
    {
        _formatStrategy = strategy;
    }
    //Метод установки стратегии
    public void SetStrategy(IFormatStrategy strategy)
    {
        _formatStrategy = strategy;
    }
    public virtual void ProcessEvent(MetricEventArgs e)
    {
        var message = FormatMessage(e.EventType, e.Data); //форматируем по стратегии
        SendMessage(message); //отправляем уведомление
        LogResult(); //логируем результат (опционально)
    }

    // Данный метод определит последовательность вызовов
    //Обратите внимание на сигнатуру
    public abstract string FormatMessage(string type, object data);
    public abstract void SendMessage(string message);
    public abstract void LogResult();
}

```

Then make specifics

```csharp
public class ConsoleHandler : EventHandlerBase
{
    IFormatStrategy _strategy;
    public ConsoleHandler(IFormatStrategy strategy) : base(strategy) =>
        _strategy = strategy;
    public override string FormatMessage(string msg, object data)
    {
        if (data is MetricData metricData)
            return _strategy.Format(msg, metricData.Timestamp);

        return _strategy.Format(msg, DateTime.Now);
    }
    public override void SendMessage(string msg) =>
        Console.WriteLine($"[Console]: {msg}");
    public override void LogResult() =>
        Console.WriteLine($"[ConsoleLog] at {DateTime.Now}");
}
```

```csharp
public class FileHandler : EventHandlerBase
{
    IFormatStrategy _strategy;
    public FileHandler(IFormatStrategy strategy) : base(strategy) =>
        _strategy = strategy;
    public override string FormatMessage(string msg, object data)
    {
        if (data is MetricData metricData)
            return _strategy.Format(msg, metricData.Timestamp);

        return _strategy.Format(msg, DateTime.Now);
    }
    public override void SendMessage(string msg) =>
        Console.WriteLine($"[File]: {msg}");
    public override void LogResult() =>
        Console.WriteLine($"[FileLog] at {DateTime.Now}");
}
```

### Program.cs

```csharp
JsonFormatStrategy jsonStrategy = new JsonFormatStrategy();
TextFormatStrategy textStrategy = new TextFormatStrategy();

ConsoleHandler consoleHandler = new ConsoleHandler(jsonStrategy);
FileHandler fileHandler = new FileHandler(textStrategy);

consoleHandler.ProcessEvent(new MetricEventArgs("myFavouriteConsoleLog", new MetricData("temp", 250, 100, DateTime.Now)));
fileHandler.ProcessEvent(new MetricEventArgs("myBelovedFileLog", new MetricData("network", 1000, 100, DateTime.Now)));
```

*Output*

```
[Console]: {"timestamp":"2026-03-28T21:12:38.3672884+07:00","message":"myFavouriteConsoleLog"}
[ConsoleLog] at 3/28/2026 9:12:38 PM
[File]: [2026-03-28 21:12:38] myBelovedFileLog
[FileLog] at 3/28/2026 9:12:38 PM
```
### Summary

I've successfully implemented Template pattern