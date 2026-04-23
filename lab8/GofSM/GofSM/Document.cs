using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GofSM
{
public class Document
    {
        public string Title { get; set; }
        public string DocumentId { get; private set; }
        private IDocumentState _state;
        public DateTime TimeStamp { get; init; }

        public Document(string title)
        {
            Title = title;
            DocumentId = Guid.NewGuid().ToString();
            TimeStamp = DateTime.Now;
            _state = new NewState();
            Console.WriteLine($"[Job Created] '{Title}' (ID: {DocumentId}) (TimeStamp: {TimeStamp})");
        }

        public void SetState(IDocumentState state) => _state = state;
        public IDocumentState GetCurrentState() => _state;

        public void Print() => _state.Print(this);
        public void OnPrintSuccess() => _state.OnPrintSuccess(this);
        public void OnPrintFailure() => _state.OnPrintFailure(this);
        public void Reset() => _state.Reset(this);
        public string GetStateName() => _state.GetType().Name;
    }
}
