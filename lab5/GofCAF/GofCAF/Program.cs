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

        IFileSystem fileSystem = new FileSystemAdapter(root);

        Console.WriteLine($"\nList of {root.Name}:");
        var rootItems = fileSystem.ListItems("/");
        foreach (var item in rootItems)
        {
            Console.WriteLine($"|-{item}");
        }

        void ShowList(string path)
        {
            Console.WriteLine($"\nList of {path}:");
  
                foreach (var item in fileSystem.ListItems(path))
                {
                    Console.WriteLine($"|-{item}");
                }
        }

        TryExecute(() => ShowList("/yabai"));
        TryExecute(() => ShowList("/nonextistent"));

        // --- Reading & Writing ---
 
        String myPath = "/yabai/eeee";

        Console.Write($"\nWriting file: {myPath}");
        TryExecute(() => fileSystem.WriteFile(myPath, new byte[] { 1, 2, 3, 4 }));
        Console.WriteLine($"\tSuccess!");

        void ShowBytes(byte[]? data)
        {
            Console.WriteLine($"Size: {data?.Length ?? 0}");
            if(data != null)
                Console.WriteLine(string.Join(" ", data));
        }
        
        Console.WriteLine($"Reading {myPath}...");
        TryExecute(() => {
            var data = fileSystem.ReadFile(myPath);
            ShowBytes(data);
        });

        ////////////////

        myPath = "/newfile.txt";

        Console.Write($"\nWriting file {myPath}:");
        TryExecute(() => fileSystem.WriteFile("/newfile.txt", new byte[] { 1, 2, 3, 4, 7, 6 }));
        Console.WriteLine($"\tSuccess!");

        Console.WriteLine($"Reading {myPath}...");
        TryExecute(() => {
            var data = fileSystem.ReadFile(myPath);
            ShowBytes(data);
        });

        TryExecute(() => ShowList("/"));

        Console.Write($"\nDeleting {myPath}:");
        TryExecute(() => fileSystem.DeleteItem(myPath));
        Console.WriteLine($"\tSuccess!");

        TryExecute(() => ShowList("/"));
    }
}