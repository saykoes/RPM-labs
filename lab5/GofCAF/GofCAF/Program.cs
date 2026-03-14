using GofCAF.Models;
using System.Drawing;
using System.IO;
using System.Reflection;
using GofFile = GofCAF.Models.File;
internal class Program
{
    private static void Main(string[] args)
    {
        
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

        Folder root = new Folder("root");

        Folder yabaiFolder = new Folder("yabai");
        root.Add(yabaiFolder);

        GofFile yabaiFile = new GofFile("eeee");
        yabaiFolder.Add(yabaiFile);
        yabaiFolder.Add(new GofFile("unn"));

        Folder myFolder = new Folder("my");
        yabaiFolder.Add(myFolder);

        myFolder.Add(new GofFile("test"));

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
    }
}