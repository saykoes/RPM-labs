using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GofSM
{
    // When job started printing
    public class PrintingState : IDocumentState
    {
        public void Print(Document document, IMediator mediator) =>
            Console.WriteLine($"[Printing] Error: '{document.Title}' already printing");
        public void OnPrintSuccess(Document document, IMediator mediator)
        {
            Console.WriteLine($"[Printing->Done] '{document.Title}' successfully completed");
            document.SetState(new DoneState());
        }
        public void OnPrintFailure(Document document, IMediator mediator)
        {
            Console.WriteLine($"[Printing->Error] Something went wrong printing '{document.Title}'");
            document.SetState(new ErrorState());
        }
        public void Reset(Document document, IMediator mediator) =>
            Console.WriteLine($"[Printing] Error: Job can't be reseted when printing");
        public void AddToQueue(Document document, IMediator mediator) =>
            Console.WriteLine($"[Printing] Cannot enqueue: job is already beign printed");
    }
}
