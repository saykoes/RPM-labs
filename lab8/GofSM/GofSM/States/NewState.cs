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
        public void Print(Document document)
        {
            Console.WriteLine($"[New->Printing] '{document.Title}' is now printing...");
            document.SetState(new PrintingState());
        }
        public void OnPrintSuccess(Document document) =>
            Console.WriteLine($"[New] Error: job hasn't stated printing");
        public void OnPrintFailure(Document document) =>
            Console.WriteLine($"[New] Error: job hasn't stated printing");
        public void Reset(Document document) =>
            Console.WriteLine($"[New] Error: new job is already reset");
    }
}
