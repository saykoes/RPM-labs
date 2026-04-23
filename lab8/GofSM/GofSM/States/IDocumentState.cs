using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GofSM
{
    public interface IDocumentState
    {
        // change to Print
        void Print(Document document, IMediator mediator);
        // change to Complete
        void OnPrintSuccess(Document document, IMediator mediator);
        // change to Fail
        void OnPrintFailure(Document document, IMediator mediator);
        // try again
        void Reset(Document document, IMediator mediator);
        void AddToQueue(Document document, IMediator mediator);
    }
}
