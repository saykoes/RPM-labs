using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GofSM
{
    public interface IMediator
    {
        void Notify(Colleague sender, string ev, Document? document = null);
    }

    public class PrintSystemMediator : IMediator
    {
        private readonly Printer _printer;
        private readonly PrintQueue _queue;
        private readonly Logger _logger;

        public PrintSystemMediator(Printer printer, PrintQueue queue, Logger logger)
        {
            _printer = printer;
            _queue = queue;
            _logger = logger;

            _printer.SetMediator(this);
            _queue.SetMediator(this);
            _logger.SetMediator(this);
        }

        public void Notify(Colleague sender, string ev, Document? document)
        {
            switch (ev)
            {
                case "AddToQueue":
                    _queue.EnqueueItem(document);
                    break;

                case "Enqueued":
                    _logger.WriteMessage($"Job '{document.Title}' is now in queue");
                    break;

                case "RequestPrint":
                    _logger.WriteMessage($"Job '{document.Title}' has been started printing");
                    _printer.StartPrint(document);
                    break;

                case "ProcessQueue":
                    ProcessQueue();
                    break;

                case "PrintSuccess":
                    document.OnPrintSuccess();
                    _logger.WriteMessage($"Job '{document.Title}' completed successfully");
                    break;

                case "PrintFailure":
                    document.OnPrintFailure();
                    _logger.WriteMessage($"Error happened (Job: '{document.Title}')");
                    break;

                case "DocumentReset":
                    _logger.WriteMessage($"Job '{document.Title}' has been resetted and added to the queue again");
                    document.AddToQueue();
                    break;

                default:
                    _logger.WriteMessage($"Undefined action: {ev} (Job: '{document.Title}')");
                    break;
            }
        }

        private void ProcessQueue()
        {
            if (_queue.IsEmpty)
            {
                _logger.WriteMessage("Queue is empty");
                return;
            }

            var nextDoc = _queue.DequeueItem();
            _logger.WriteMessage($"Dequeued job '{nextDoc.Title}'");
            nextDoc.Print();
        }
    }
}
