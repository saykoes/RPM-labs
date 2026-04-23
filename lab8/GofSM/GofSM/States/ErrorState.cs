using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GofSM
{
    public class ErrorState : IDocumentState
    {
        public void Print(Document document, IMediator mediator) =>
            Console.WriteLine($"[Error] Job can't be printed. Reset the job first");
        public void OnPrintSuccess(Document document, IMediator mediator) =>
            Console.WriteLine($"[Error] Failed job can't be completed");
        public void OnPrintFailure(Document document, IMediator mediator) =>
            Console.WriteLine($"[Error] Job already failed");
        public void Reset(Document document, IMediator mediator)
        {
            Console.WriteLine($"[Error->New] Job '{document.Title}' resetted");
            document.SetState(new NewState());
            mediator.Notify(document, "DocumentReset", document);
        }
        public void AddToQueue(Document document, IMediator mediator) =>
            Console.WriteLine($"[Error] Cannot enqueue job without resetting");
    }
}
