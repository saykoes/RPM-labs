using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GofSM
{
    public abstract class Colleague
    {
        protected IMediator _mediator;
        public void SetMediator(IMediator mediator) =>
            _mediator = mediator;
    }
    public class Logger : Colleague
    {
        public void WriteMessage(string message)
        {
            string timestamp = DateTime.Now.ToString();
            Console.WriteLine($"[Log: {timestamp}] {message}");
        }
    }
    public class Printer : Colleague
    {
        public bool SimulateFailure { get; set; } = false;

        public void StartPrint(Document document)
        {
            Console.WriteLine($"[Printer] printing '{document.Title}'...");

            if (SimulateFailure)
            {
                SimulateFailure = false;
                Console.WriteLine($"[Printer] ERROR");
                _mediator.Notify(this, "PrintFailure", document);
            }
            else
            {
                Console.WriteLine($"[Printer] printed successfully");
                _mediator.Notify(this, "PrintSuccess", document);
            }
        }
    }
    public class PrintQueue : Colleague
    {
        private Queue<Document> _documents = new Queue<Document>();
        public void EnqueueItem(Document document)
        {
            _documents.Enqueue(document);
            Console.WriteLine($"[Queue] Job '{document.Title}' was added to queue (total: {_documents.Count})");
            _mediator.Notify(this, "Enqueued", document);
        }
        public Document DequeueItem() => _documents.Dequeue();
        public bool IsEmpty => _documents.Count == 0;
        public int GetCount() => _documents.Count;
    }
    public class Dispatcher : Colleague
    {
        public void CommandProcessQueue()
        {
            Console.WriteLine("[Dispatch] Executing queue...");
            _mediator.Notify(this, "ProcessQueue");
        }
    }
}
