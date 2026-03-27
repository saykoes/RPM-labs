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


### Practice

### Program.cs
```csharp

```

*Output*
```

```

### Summary
I've successfully implemented Strategy pattern

---

## Step 3: Template Method

### Theory


### Practice


### Program.cs

```csharp

```

*Output*

```

```
### Summary

I've successfully implemented Template pattern