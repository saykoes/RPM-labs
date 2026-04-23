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
        void Print(Document document);
        // change to Complete
        void OnPrintSuccess(Document document);
        // change to Fail
        void OnPrintFailure(Document document);
        // try again
        void Reset(Document document);
    }
}
