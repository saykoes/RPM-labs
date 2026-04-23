## Lab 8. GOF: State, Mediator
## Поведенческие паттерны проектирования. Состояние, Посредник
### Цель работы: 
Формирование практических навыков применения поведенческих
паттернов проектирования в рамках единой программной системы.
### Задание:
Необходимо реализовать консольное приложение — простую систему
управления очередью печати документов. Система включает:
- Документ, который может находиться в разных состояниях и менять поведение в зависимости от состояния.
- Принтер, который может печатать один документ за раз, может «ломаться» (ошибка) и снова «чиниться».
- Очередь документов (FIFO), ожидающих печати.
- Логгер, который фиксирует ключевые события (документ добавлен, печать начата, ошибка, успешная печать и т.п.).
- Простой диспетчер (UI‐уровня), который добавляет документы и запускает печать.

--- 

## Step 1: State

### Theory

We want to have states that logically flow from one to another. We can utilize State pattern. It has an interface of all states and concrete implementations

### Practice

Interface of all states
```csharp
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
```
Concrete implementations
```csharp
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
```
```csharp
// When job started printing
public class PrintingState : IDocumentState
{
    public void Print(Document document) =>
        Console.WriteLine($"[Printing] Error: '{document.Title}' already printing");
    public void OnPrintSuccess(Document document)
    {
        Console.WriteLine($"[Printing->Done] '{document.Title}' successfully completed");
        document.SetState(new DoneState());
    }
    public void OnPrintFailure(Document document)
    {
        Console.WriteLine($"[Printing->Error] Something went wrong printing '{document.Title}'");
        document.SetState(new ErrorState());
    }
    public void Reset(Document document) =>
        Console.WriteLine($"[Printing] Error: Job can't be reseted when printing");
}
```
```csharp
public class ErrorState : IDocumentState
{
    public void Print(Document document) =>
        Console.WriteLine($"[Error] Job can't be printed. Reset the job first");
    public void OnPrintSuccess(Document document) =>
        Console.WriteLine($"[Error] Failed job can't be completed");
    public void OnPrintFailure(Document document) =>
        Console.WriteLine($"[Error] Job already failed");
    public void Reset(Document document)
    {
        Console.WriteLine($"[Error->New] Job '{document.Title}' reseted");
        document.SetState(new NewState());
    }
}
```

```csharp
public class DoneState : IDocumentState
{
    public void Print(Document document) =>
        Console.WriteLine($"[Done] Job '{document.Title}' already completed");
    public void OnPrintSuccess(Document document) =>
        Console.WriteLine($"[Done] Job '{document.Title}' already completed");
    public void OnPrintFailure(Document document) =>
        Console.WriteLine($"[Done] Job '{document.Title}' already completed");
    public void Reset(Document document) =>
        Console.WriteLine($"[Done] Job '{document.Title}' already completed, can't reset");
}
```

We need to create a printing job class
```csharp
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
```

### In Program.cs

```csharp
Document doc = new Document("MyDocumetb");

doc.Print();
doc.Print(); // Try to print the same job again
doc.OnPrintSuccess(); // Print complete
doc.Print(); // Try to print completed job
Console.WriteLine();

Document errorDoc = new Document("ErDoc444");

errorDoc.Print();
errorDoc.OnPrintFailure(); // Oh no! An error!
errorDoc.Print(); // Try to print wo/ resetting
errorDoc.Reset(); // Resetting
errorDoc.Print(); // Here we go
errorDoc.OnPrintSuccess();// Completed
```

*Output*
```
[Job Created] 'MyDocumetb' (ID: b9b94e13-7615-4616-8fd0-b42c542d590e) (TimeStamp: 23.04.2026 14:03:56)
[New->Printing] 'MyDocumetb' is now printing...
[Printing] Error: 'MyDocumetb' already printing
[Printing->Done] 'MyDocumetb' successfully completed
[Done] Job 'MyDocumetb' already completed

[Job Created] 'ErDoc444' (ID: 67466dfa-3d04-412a-9eaf-ddfff740c84c) (TimeStamp: 23.04.2026 14:03:56)
[New->Printing] 'ErDoc444' is now printing...
[Printing->Error] Something went wrong printing 'ErDoc444'
[Error] Job can't be printed. Reset the job first
[Error->New] Job 'ErDoc444' reseted
[New->Printing] 'ErDoc444' is now printing...
[Printing->Done] 'ErDoc444' successfully completed
```

### Summary

I've successfully implemented State pattern

---

## Step 2: Mediator

### Theory

What if wanted to have an orcestrator that would work with everyone (colleagues) but everyone would talk ONLY with the orchestrator. That is called a Mediator pattern

### Practice

```csharp
public interface IMediator
{
    void Notify(Colleague sender, string ev, Document? document = null);
}
```

```csharp
public abstract class Colleague
{
    protected IMediator _mediator;
    public void SetMediator(IMediator mediator) =>
        _mediator = mediator;
}
```
```csharp
public class Logger : Colleague
{
    public void WriteMessage(string message)
    {
        string timestamp = DateTime.Now.ToString();
        Console.WriteLine($"[Log: {timestamp}] {message}");
    }
}
```
```csharp
public class Dispatcher : Colleague
{
    public void CommandProcessQueue()
    {
        Console.WriteLine("[Dispatch] Executing queue...");
        _mediator.Notify(this, "ProcessQueue");
    }
}
```
```csharp
public class Printer : Colleague
{
    public bool SimulateFailure { get; set; } = false;

    public void StartPrint(Document document)
    {
        Console.WriteLine($"[Printer] printing '{document.Title}'...");

        if (SimulateFailure)
        {
            SimulateFailure = false;
            Console.WriteLine($"[Printer] ERROR");
            _mediator.Notify(this, "PrintFailure", document);
        }
        else
        {
            Console.WriteLine($"[Printer] printed successfully");
            _mediator.Notify(this, "PrintSuccess", document);
        }
    }
}
```
```csharp
public class PrintQueue : Colleague
{
    private Queue<Document> _documents = new Queue<Document>();
    public void EnqueueItem(Document document)
    {
        _documents.Enqueue(document);
        Console.WriteLine($"[Queue] Job '{document.Title}' was added to queue (total: {_documents.Count})");
        _mediator.Notify(this, "Enqueued", document);
    }
    public Document DequeueItem() => _documents.Dequeue();
    public bool IsEmpty => _documents.Count == 0;
    public int GetCount() => _documents.Count;
}
```

Now let's implement IMediator interface
```csharp
public class PrintSystemMediator : IMediator
{
    private readonly Printer _printer;
    private readonly PrintQueue _queue;
    private readonly Logger _logger;

    public PrintSystemMediator(Printer printer, PrintQueue queue, Logger logger)
    {
        _printer = printer;
        _queue = queue;
        _logger = logger;

        _printer.SetMediator(this);
        _queue.SetMediator(this);
        _logger.SetMediator(this);
    }

    public void Notify(Colleague sender, string ev, Document? document)
    {
        switch (ev)
        {
            case "AddToQueue":
                _queue.EnqueueItem(document);
                break;

            case "Enqueued":
                _logger.WriteMessage($"Job '{document.Title}' is now in queue");
                break;

            case "RequestPrint":
                _logger.WriteMessage($"Job '{document.Title}' has been started printing");
                _printer.StartPrint(document);
                break;

            case "ProcessQueue":
                ProcessQueue();
                break;

            case "PrintSuccess":
                document.OnPrintSuccess();
                _logger.WriteMessage($"Job '{document.Title}' completed successfully");
                break;

            case "PrintFailure":
                document.OnPrintFailure();
                _logger.WriteMessage($"Error happened (Job: '{document.Title}')");
                break;

            case "DocumentReset":
                _logger.WriteMessage($"Job '{document.Title}' has been resetted and added to the queue again");
                document.AddToQueue();
                break;

            default:
                _logger.WriteMessage($"Undefined action: {ev} (Job: '{document.Title}')");
                break;
        }
    }

    private void ProcessQueue()
    {
        if (_queue.IsEmpty)
        {
            _logger.WriteMessage("Queue is empty");
            return;
        }

        var nextDoc = _queue.DequeueItem();
        _logger.WriteMessage($"Dequeued job '{nextDoc.Title}'");
        nextDoc.Print();
    }
}
```

let's make old parts work with mediator and queue

Document now is colleague
```csharp
public class Document : Colleague
{
    // ...
    public Document(string title, IMediator mediator)
    {
        // ...
        SetMediator(mediator);
        // ...
    }
    // ...
    public void AddToQueue() => _state.AddToQueue(this, _mediator);
}
```

```csharp
public interface IDocumentState
{
    // ...
    void AddToQueue(Document document, IMediator mediator);
}
```
```csharp
public class NewState : IDocumentState
{
    // ...
    public void Print(Document document, IMediator mediator)
    {
        // ...
        mediator.Notify(document, "RequestPrint", document);
    }
    // ...
    public void AddToQueue(Document document, IMediator mediator)
    {
        Console.WriteLine($"[New] Adding Job '{document.Title}' to queue...");
        mediator.Notify(document, "AddToQueue", document);
    }
}
```
```csharp
public class PrintingState : IDocumentState
{
    // ...
    public void AddToQueue(Document document, IMediator mediator) =>
    Console.WriteLine($"[Printing] Cannot enqueue: job is already beign printed");
}
```
```csharp
public class ErrorState : IDocumentState
{
    // ...
    public void Reset(Document document, IMediator mediator)
    {
        // ...
        mediator.Notify(document, "DocumentReset", document);
    }
    public void AddToQueue(Document document, IMediator mediator) =>
        Console.WriteLine($"[Error] Cannot enqueue job without resetting");
}
```
```csharp
public class DoneState : IDocumentState
{
    // ...
    public void AddToQueue(Document document, IMediator mediator) =>
        Console.WriteLine($"[Done] Cannot enqueue completed job");  
}
```

### In Program.cs

```csharp
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
```

*Output*
```
[Job Created] 'Document1' (ID: 575a0680-d5bc-4269-8463-504546125348) (TimeStamp: 23.04.2026 16:07:34)
[Job Created] 'Document2' (ID: 3bef5870-8fbf-464e-ad10-5b492e94ea79) (TimeStamp: 23.04.2026 16:08:27)
[Job Created] 'Document3' (ID: 8cc8cca5-c779-4244-b52a-da5c663f8807) (TimeStamp: 23.04.2026 16:08:27)

[New] Adding Job 'Document1' to queue...
[Queue] Job 'Document1' was added to queue (total: 1)
[Log: 23.04.2026 16:08:27] Job 'Document1' is now in queue
[New] Adding Job 'Document2' to queue...
[Queue] Job 'Document2' was added to queue (total: 2)
[Log: 23.04.2026 16:08:27] Job 'Document2' is now in queue
[New] Adding Job 'Document3' to queue...
[Queue] Job 'Document3' was added to queue (total: 3)
[Log: 23.04.2026 16:08:27] Job 'Document3' is now in queue

[Dispatch] Executing queue...
[Log: 23.04.2026 16:08:27] Dequeued job 'Document1'
[New->Printing] 'Document1' is now printing...
[Log: 23.04.2026 16:08:27] Job 'Document1' has been started printing
[Printer] printing 'Document1'...
[Printer] printed successfully
[Printing->Done] 'Document1' successfully completed
[Log: 23.04.2026 16:08:27] Job 'Document1' completed successfully

[Dispatch] Executing queue...
[Log: 23.04.2026 16:08:27] Dequeued job 'Document2'
[New->Printing] 'Document2' is now printing...
[Log: 23.04.2026 16:08:27] Job 'Document2' has been started printing
[Printer] printing 'Document2'...
[Printer] ERROR
[Printing->Error] Something went wrong printing 'Document2'
[Log: 23.04.2026 16:08:27] Error happened (Job: 'Document2')
[Error->New] Job 'Document2' resetted
[Log: 23.04.2026 16:08:27] Job 'Document2' has been resetted and added to the queue again
[New] Adding Job 'Document2' to queue...
[Queue] Job 'Document2' was added to queue (total: 2)
[Log: 23.04.2026 16:08:27] Job 'Document2' is now in queue
[Dispatch] Executing queue...
[Log: 23.04.2026 16:08:27] Dequeued job 'Document3'
[New->Printing] 'Document3' is now printing...
[Log: 23.04.2026 16:08:27] Job 'Document3' has been started printing
[Printer] printing 'Document3'...
[Printer] printed successfully
[Printing->Done] 'Document3' successfully completed
[Log: 23.04.2026 16:08:27] Job 'Document3' completed successfully

[Dispatch] Executing queue...
[Log: 23.04.2026 16:08:27] Dequeued job 'Document2'
[New->Printing] 'Document2' is now printing...
[Log: 23.04.2026 16:08:27] Job 'Document2' has been started printing
[Printer] printing 'Document2'...
[Printer] printed successfully
[Printing->Done] 'Document2' successfully completed
[Log: 23.04.2026 16:08:27] Job 'Document2' completed successfully
[Done] Job 'Document3' already completed
```

### Summary
I've successfully implemented Mediator pattern