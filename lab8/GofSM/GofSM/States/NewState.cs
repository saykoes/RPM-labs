using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GofSM
{
    // When job has been created
    public class NewState : IDocumentState
    {
        public void Print(Document document, IMediator mediator)
        {
            Console.WriteLine($"[New->Printing] '{document.Title}' is now printing...");
            document.SetState(new PrintingState());
            mediator.Notify(document, "RequestPrint", document);
        }
        public void OnPrintSuccess(Document document, IMediator mediator)
        {
            Console.WriteLine($"[New] Error: job hasn't stated printing");
        }
        public void OnPrintFailure(Document document, IMediator mediator) =>
            Console.WriteLine($"[New] Error: job hasn't stated printing");
        public void Reset(Document document, IMediator mediator) =>
            Console.WriteLine($"[New] Error: new job is already reset");
        public void AddToQueue(Document document, IMediator mediator)
        {
            Console.WriteLine($"[New] Adding Job '{document.Title}' to queue...");
            mediator.Notify(document, "AddToQueue", document);
        }
    }
}
