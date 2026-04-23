using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GofSM
{
    public class DoneState : IDocumentState
    {
        public void Print(Document document, IMediator mediator) =>
            Console.WriteLine($"[Done] Job '{document.Title}' already completed");
        public void OnPrintSuccess(Document document, IMediator mediator) =>
            Console.WriteLine($"[Done] Job '{document.Title}' already completed");
        public void OnPrintFailure(Document document, IMediator mediator) =>
            Console.WriteLine($"[Done] Job '{document.Title}' already completed");
        public void Reset(Document document, IMediator mediator) =>
            Console.WriteLine($"[Done] Job '{document.Title}' already completed, can't reset");
        public void AddToQueue(Document document, IMediator mediator) =>
            Console.WriteLine($"[Done] Cannot enqueue completed job");
    }
}
