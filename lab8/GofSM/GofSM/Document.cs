using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GofSM
{
    public class Document : Colleague
    {
        public string Title { get; set; }
        public string DocumentId { get; private set; }
        private IDocumentState _state;
        public DateTime TimeStamp { get; init; }

        public Document(string title, IMediator mediator)
        {
            Title = title;
            DocumentId = Guid.NewGuid().ToString();
            TimeStamp = DateTime.Now;            
            _state = new NewState();
            SetMediator(mediator);
            Console.WriteLine($"[Job Created] '{Title}' (ID: {DocumentId}) (TimeStamp: {TimeStamp})");
        }

        public void SetState(IDocumentState state) => _state = state;
        public IDocumentState GetCurrentState() => _state;

        public void Print() => _state.Print(this, _mediator);
        public void OnPrintSuccess() => _state.OnPrintSuccess(this, _mediator);
        public void OnPrintFailure() => _state.OnPrintFailure(this, _mediator);
        public void Reset() => _state.Reset(this, _mediator);
        public string GetStateName() => _state.GetType().Name;
        public void AddToQueue() => _state.AddToQueue(this, _mediator);
    }
}
