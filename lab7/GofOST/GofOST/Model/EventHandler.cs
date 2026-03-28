using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GofOST.Model
{
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
        // Данный метод определит последовательность вызовов
        //Обратите внимание на сигнатуру
        public virtual void ProcessEvent(MetricEventArgs e)
        {
            var message = FormatMessage(e.EventType, e.Data); //форматируем по стратегии
            SendMessage(message); //отправляем уведомление
            LogResult(); //логируем результат (опционально)
        }
        public abstract string FormatMessage(string type, object data);
        public abstract void SendMessage(string message);
        public abstract void LogResult();
    }

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
}
