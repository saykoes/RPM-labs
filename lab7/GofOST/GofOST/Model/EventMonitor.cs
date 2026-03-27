using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GofOST.Model
{
    public class MetricEventArgs(string eventType, MetricData data) : EventArgs
    {
        public string EventType { get; } = eventType ?? 
            throw new ArgumentNullException(nameof(eventType));
        public MetricData Data { get; } = data ?? 
            throw new ArgumentNullException(nameof(data));
    }
    public delegate void MetricEventHandler(MetricEventArgs e);

    /// <summary>
    /// Субъект (Subject). Вместо интерфейса ISubject и методов Attach/Detach
    /// использует стандартное событие .NET.
    /// </summary>
    internal class EventMonitor
    {
        // Событие (Event) — это ключевой элемент паттерна Observer в C#
        public event MetricEventHandler? OnMetricExceeded;
        public void CheckMetric(string metricName = "Temperature", double value = 110, double threshold = 100)
        {
            Console.WriteLine($"[Monitor]: '{metricName}' (Value: {value}; Threshold: {threshold})");
            if (value > threshold)
            {
                // Вместо цикла foreach в методе Notify, мы просто вызываем событие.
                // ?.Invoke проверяет, есть ли подписчики.
                MetricData eventData = new MetricData(metricName, value, threshold, DateTime.Now);
                OnMetricExceeded?.Invoke(new MetricEventArgs(eventType: metricName +
                "_Exceeded", data: eventData));
            }
        }
    }
}
