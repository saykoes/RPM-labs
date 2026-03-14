## Lab 5. GOF: Composite, Adapter, Facade
## Структурные паттерны проектирования. Компоновщик, Адаптер, Фасад
### Цель работы: 
Изучить структурные паттерны проектирования GoF на примере разработки приложения, моделирующего работу
файлового менеджера с поддержкой различных файловых систем и облачных хранилищ.
Освоить на практике паттерны **Composite** для представления иерархической структуры файлов и папок, **Adapter** для унификации доступа к разным файловым системам (локальная, FTP, облачные API) и **Facade** для предоставления простого интерфейса выполнения типовых операций (синхронизация, резервное копирование).
### Задание:
Разработать консольное приложение, моделирующее работу с
файловой системой:

Приложение должно позволять:
- Строить иерархическую структуру из файлов и папок, используя паттерн **Composite** (файлы – листья, папки – компоновщики).
- Реализовать операции рекурсивного обхода (подсчёт размера, удаление,копирование). 
Интегрировать разные файловые системы через паттерн **Adapter**, предоставляющий единый интерфейс для основных операций (список содержимого, чтение, запись, удаление).  
- Предоставить простой интерфейс для типовых сценариев через паттерн **Facade** (например, синхронизация локальной папки с облаком, создание резервной копии), скрывающий сложность рекурсивного обхода и вызовов адаптеров.

--- 

## Step 1: Composite

### Theory

Classes `File` and `Folder` are children of `FileSystemItem` class
- `FileSystemItem` is general component
- `File` is leaf (it cannot have other leaves)
- `Folder` is comopsite (it can have leaves)

This way, Component pattern is pretty much a tree

### Practice

I've implemented `FileSystemItem` class

```csharp
public abstract class FileSystemItem
{
    public string Name { get; protected set; }
    public FileSystemItem(string name) => Name = name;
    public abstract long Size { get; }
    public abstract void Add(FileSystemItem item);
    public abstract void Remove(FileSystemItem item);
}
```

`File` class

```csharp
public class File : FileSystemItem
{
    public override long Size => Data?.Length ?? 0;
    public byte[]? Data { get; set; }
    public File(string name) : base(name) { }
    public override void Add(FileSystemItem item) => 
        throw new InvalidOperationException("File cannot have children");
    public override void Remove(FileSystemItem item) => 
        throw new InvalidOperationException("File cannot have children");
}
```
Because `File` is not implementing `Add` and `Remove` methods, it's a LSP violation.
We can actually remove them from the base class because we are still going to check for item types (which is an OCP violation by itself) when I'm going to implement Adapter pattern later. Or, we could've embraced it by not checking for types and catching exceptions instead

`Folder` class
```csharp
public class Folder : FileSystemItem
{
    protected readonly List<FileSystemItem> _children;
    public IReadOnlyList<FileSystemItem> Children => _children.AsReadOnly();

    public Folder(string name) : base(name) => _children = new List<FileSystemItem>();

    public override long Size => _children.Sum(c => c.Size);
    public override void Add(FileSystemItem item)
    {
        if (_children.Any(i => i.Name == item.Name))
            throw new InvalidOperationException("Item with this name already exists");

        _children.Add(item);
    }
    public override void Remove(FileSystemItem item)
    {
        if (!_children.Contains(item))
            throw new InvalidOperationException("File not found");
        
        _children.Remove(item);
    }
}
```
Getting size of Folder recursively by using `_children.Sum`. <br> 
I was inspired by a bunch of `InvalidOperationException` in `File` class so I'm using it here to throw errors

### In Program.cs
```csharp
void TryExecute(Action action)
{
    try
    {
        action();
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"ERROR: {ex.Message}");
    }
}
```
We threw a lot of exceptions, so now we need to catch 'em all. Using `Action` delegate here to not type try/catch statement in the code every time

Let's test everything
```csharp
Folder root = new Folder("root");

Folder yabaiFolder = new Folder("yabai");
root.Add(yabaiFolder);

GofFile yabaiFile = new GofFile("eeee");
yabaiFolder.Add(yabaiFile);
yabaiFolder.Add(new GofFile("unn"));

Folder myFolder = new Folder("my");
yabaiFolder.Add(myFolder);

myFolder.Add(new GofFile("test"));

TryExecute(() => yabaiFolder.Add(new Folder("my")));
TryExecute(() => myFolder.Add(new GofFile("test")));
TryExecute(() => yabaiFile.Add(new GofFile("test")));

TryExecute(() => myFolder.Remove(myFolder.Children.FirstOrDefault()));
TryExecute(() => myFolder.Remove(myFolder.Children.FirstOrDefault()));
```

*Output*
```
ERROR: Item with this name already exists
ERROR: Item with this name already exists
ERROR: File cannot have children
ERROR: File not found
```

### Summary

I've successfully implemented Composite pattern

---

## Step 2: Adapter

### Theory

But what if we had an interface that expects different methods? 
```csharp
internal interface IFileSystem
{
    List<string> ListItems(string path);
    byte[]? ReadFile(string path);
    void WriteFile(string path, byte[] data);
    void DeleteItem(string path);
}
```
We can implement an Adapter method that will be a middle man between client and our `FileSytemItem`

```csharp
internal class FileSystemAdapter : IFileSystem
```

### Practice

`IFileSystem` expects us to work with string paths. Let's introduce root directory
```csharp
internal class FileSystemAdapter : IFileSystem
{
    private FileSystemItem _root;
    public FileSystemAdapter(FileSystemItem root) => _root = root;
```
Since we work with paths, we now need somehow to get an actual `FileSystemItem` object
```csharp
    private FileSystemItem FindByPath(string path)
    {
        // We split path in parts
        var parts = path?.Trim().Split('/', '\\').Where(p => !string.IsNullOrEmpty(p)) ?? Enumerable.Empty<string>();
        
        // and just follow them until we hit the end 
        FileSystemItem current = _root;
        foreach (var part in parts)
        {
            // find a child with an exact name as part
            if (current is Folder folder && folder.Children.FirstOrDefault(c => c.Name == part) is FileSystemItem found)
                current = found;
            else
                throw new InvalidOperationException($"{path}: '{part}' not found");
        }

        return current;
    }
```


Now we have to implement `ListItems` method
```csharp
    public List<string> ListItems(string path)
    {
        var result = new List<string>();
        var item = FindByPath(path); // We find a `FileSystemItem` by path

        if (item is Folder folder)
        {
            foreach (var child in folder.Children) // check its children
            {
                // stylize an output a bit
                if (child is File)
                    result.Add($"[File] \t{child.Name} \t{child.Size} Bytes");
                else if (child is Folder)
                    result.Add($"[Folder] \t{child.Name}");
            }
        }
        else
            result.Add($"[File] \t{item.Name} \t{item.Size} Bytes");

        return result; // and return a List of result Strings
    }
```

Now, `ReadFile`
```csharp
    public byte[]? ReadFile(string path)
    {
        var item = FindByPath(path);

        if (item is File file)
            return file.Data; // We just return data of the found `FileSystemItem`
        
        throw new InvalidOperationException($"{path}: '{item.Name}' is not a File");
    }
```

`WriteFile`
```csharp
    public void WriteFile(string path, byte[] data)
    {
        // Split the path to a file
        var parts = path.Split('/', '\\').Where(p => !string.IsNullOrEmpty(p)).ToList();
        if (!parts.Any()) return;   // do nothing if path is empty

        string fileName = parts.Last(); // get File name from path
        var current = (Folder)_root;
        
        // follow the path
        foreach (var part in parts.SkipLast(1)) // skip last element because it's a `File`
        {
            // find a child with an exact name as part
            var existing = current.Children.FirstOrDefault(c => c.Name == part); 

            // create new Folder if non-existent
            if (existing == null)
            {
                existing = new Folder(part);
                current.Add(existing);
            }

            if (existing is Folder folder)
                current = folder;

            else
                throw new InvalidOperationException($"{path}: '{part}' is not a Folder and already exists");
        }

        // find a child with an exact name as file
        var item = current.Children.FirstOrDefault(c => c.Name == fileName);
        
        //  create if non-existent
        if (item == null)
        {
            item = new File(fileName);
            current.Add(item);
        }

        if (item is File file)
            file.Data = data?.ToArray(); // Write to file 
        else
            throw new InvalidOperationException($"{path}: '{item.Name}' is not a File and already exists");    
    }
```

`DeleteItem`
```csharp
    public void DeleteItem(string path)
    {
        // check for root
        if (path.Trim() == "/" || path.Trim() == "\\" || path == "") 
            throw new InvalidOperationException("Cannot delete root"); 

        // get the parent path
        int lastSlash = path.LastIndexOf('/');
        string parentPath = lastSlash <= 0 ? "/" : path.Substring(0, lastSlash);

        var parent = FindByPath(parentPath) as Folder;
        var item = FindByPath(path);

        // remove an item (garbage collector will remove all child elements as long as it doesn't have link to other object)
        parent?.Remove(item);
    }
}
```

### Program.cs
Now let's check functionality
```csharp
// ... Using the same file/folder structure as in previous iteration (only without Remove)...
// --- Filesystem Adapter ---

IFileSystem localStorage = new FileSystemAdapter(root);

Console.WriteLine($"\nList of {root.Name}:");
var rootItems = localStorage.ListItems("/");
foreach (var item in rootItems)
{
    Console.WriteLine($"|-{item}");
}

void ShowList(IFileSystem fileSystem,string path)
{
    Console.WriteLine($"\nList of {path}:");

        foreach (var item in fileSystem.ListItems(path))
        {
            Console.WriteLine($"|-{item}");
        }
}

TryExecute(() => ShowList(localStorage, "/yabai"));
TryExecute(() => ShowList(localStorage, "/nonextistent"));

// --- Reading & Writing ---

String myPath = "/yabai/eeee";

Console.Write($"\nWriting file: {myPath}");
TryExecute(() => localStorage.WriteFile(myPath, new byte[] { 1, 2, 3, 4 }));
Console.WriteLine($"\tSuccess!");

void ReadBytes(IFileSystem fs, String path)
{
    var data = fs.ReadFile(path);
    Console.WriteLine($"\nReading {path} (Size: {data?.Length ?? 0} Bytes)...");
    if(data != null)
        Console.WriteLine(string.Join(" ", data));
}

TryExecute(() => {
    ReadBytes(localStorage,myPath);
});

////////////////

myPath = "/newfile.txt";

Console.WriteLine($"\nWriting file '{myPath}'...");
TryExecute(() => localStorage.WriteFile("/newfile.txt", new byte[] { 1, 2, 3, 4, 7, 6 }));
Console.WriteLine($"OK");

TryExecute(() => {
    ReadBytes(localStorage, myPath);
});

TryExecute(() => ShowList(localStorage, "/"));

Console.Write($"\nDeleting {myPath}:");
TryExecute(() => localStorage.DeleteItem(myPath));
Console.WriteLine($"\tSuccess!");

TryExecute(() => ShowList(localStorage, "/"));
```

*Output*
```
List of root:
|-[Folder]      yabai

List of /yabai:
|-[File]        eeee    0 Bytes
|-[File]        unn     0 Bytes
|-[Folder]      my

List of /nonextistent:
ERROR: /nonextistent: 'nonextistent' not found

Writing file: /yabai/eeee       Success!

Reading /yabai/eeee (Size: 4 Bytes)...
1 2 3 4

Writing file '/newfile.txt'...
OK

Reading /newfile.txt (Size: 6 Bytes)...
1 2 3 4 7 6

List of /:
|-[Folder]      yabai
|-[File]        newfile.txt     6 Bytes

Deleting /newfile.txt:  Success!

List of /:
|-[Folder]      yabai
```
### Summary
I've successfully implemented Adapter pattern

---

## Step 3: Facade

### Theory

But what if I want to sync this filesystem to another? I can use Facade pattern which uses adapters to work with file systems

### Practice
Let's create our `SyncFacade` class and introduce 2 filesystems
```csharp
internal class SyncFacade
{
    private IFileSystem _sourceFS;
    private IFileSystem _targetFS;

    public SyncFacade(IFileSystem source, IFileSystem target)
    {
        _sourceFS = source;
        _targetFS = target;
    }
```

I wanted to have logs so let's create delegate to store actual logging action in it later

```csharp
    // For logging
    public event Action<string>? OnLog;
    private void Log(string message) => OnLog?.Invoke(message); // Invoke because OnLog is nullable
```

Now we need to implement syncing method
```csharp
    public void SyncFolder(string sourcePath, string targetPath)
    {
        var items = _sourceFS.ListItems(sourcePath); // Get folder's children
        foreach (var item in items)
        {
            string itemName = item.Split('\t')[1].Trim(); // Get the name of child
            // define actual paths
            string sourceItemPath = (sourcePath == "/") ? $"/{itemName}" : $"{sourcePath}/{itemName}"; 
            string targetItemPath = (targetPath == "/") ? $"/{itemName}" : $"{targetPath}/{itemName}";

            if (item.StartsWith("[File]"))
            {
                Log($"Copying '{itemName}'...");
                byte[] data = _sourceFS.ReadFile(sourceItemPath);
                _targetFS.WriteFile(targetItemPath, data);
                Log("OK");
            }
            else if (item.StartsWith("[Folder]"))
            {
                Log($"Going into '{itemName}/'...");
                SyncFolder(sourceItemPath, targetItemPath); // call method recursively
            }
        }
        Log("Sync Complete");
    }
```
And let's create method for backups using `SyncFolder` method
```csharp
    public void Backup(string sourcePath, string backupRootPath)
    {
        // Integrate timestap for backup name automatically
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string backupPath = $"{backupRootPath}/backup_{timestamp}"; 

        SyncFolder(sourcePath, backupPath);

        Log($"Backup Complete\n");
    }
}
```

### Program.cs
Testing

```csharp
// ... same thing ...

// --- Backup ---

Folder cloudRoot = new Folder("CloudRoot");
IFileSystem cloudStorage = new FileSystemAdapter(cloudRoot);

SyncFacade facade = new SyncFacade(localStorage, cloudStorage);
facade.OnLog += Console.WriteLine;  // Subscribe to OnLog Action delegate

cloudRoot.Add(new Folder("MyRoot"));
var backupFolder = new Folder("Backups");
cloudRoot.Add(backupFolder);

ShowList(cloudStorage, "/");

Console.WriteLine("Sync to Cloud:");
TryExecute(() => facade.SyncFolder("/", "/MyRoot"));
TryExecute(() => ShowList(cloudStorage, "/MyRoot"));

TryExecute(() => ReadBytes(cloudStorage, $"/MyRoot{myPath}"));


Console.WriteLine("\nBackup:");
TryExecute(() => facade.Backup("/yabai", "/Backups"));

ShowList(cloudStorage, "/Backups");
TryExecute(() => ShowList(cloudStorage, $"/Backups/{backupFolder.Children.FirstOrDefault()?.Name}"));

TryExecute(() => cloudStorage.DeleteItem("/Backups"));
ShowList(cloudStorage, "/");
```

*Output*

```
List of /:
|-[Folder]      MyRoot
|-[Folder]      Backups
Sync to Cloud:
Going into 'yabai/'...
Copying 'eeee'...
OK
Copying 'unn'...
OK
Going into 'my/'...
Copying 'test'...
OK
Sync Complete
Sync Complete
Copying 'newfile.txt'...
OK
Sync Complete

List of /MyRoot:
|-[Folder]      yabai
|-[File]        newfile.txt     6 Bytes

Reading /MyRoot/newfile.txt (Size: 6 Bytes)...
1 2 3 4 7 6

Backup:
Copying 'eeee'...
OK
Copying 'unn'...
OK
Going into 'my/'...
Copying 'test'...
OK
Sync Complete
Sync Complete
Backup Complete


List of /Backups:
|-[Folder]      backup_2026-03-14_18-49-49

List of /Backups/backup_2026-03-14_18-49-49:
|-[File]        eeee    4 Bytes
|-[File]        unn     0 Bytes
|-[Folder]      my

List of /:
|-[Folder]      MyRoot
```
### Summary

I've successfully implemented Facade pattern