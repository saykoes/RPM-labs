using System.Reflection;
using System.Reflection.Metadata;

namespace GofSM
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Printer printer = new Printer();
            PrintQueue queue = new PrintQueue();
            Logger logger = new Logger();

            PrintSystemMediator mediator = new PrintSystemMediator(printer, queue, logger);

            Dispatcher dispatcher = new Dispatcher();
            dispatcher.SetMediator(mediator);

            Document doc1 = new Document("Document1", mediator);
            Document doc2 = new Document("Document2", mediator);
            Document doc3 = new Document("Document3", mediator);

            Console.WriteLine();

            doc1.AddToQueue();
            doc2.AddToQueue();
            doc3.AddToQueue();

            Console.WriteLine();

            // 1. Successful print
            dispatcher.CommandProcessQueue();
            Console.WriteLine();

            // 2. Printer error and recovery
            printer.SimulateFailure = true;
            dispatcher.CommandProcessQueue();
            doc2.Reset(); // Resetting after a failure
            dispatcher.CommandProcessQueue();
            Console.WriteLine();

            // 3. Check final state
            dispatcher.CommandProcessQueue();
            doc3.Print(); // Try to print already printed document
            Console.WriteLine();
        }
    }
}
