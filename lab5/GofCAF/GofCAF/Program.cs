using GofCAF.Models;
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

        Folder yabaiFolder = new Folder("yabai");

        GofFile yabaiFile = new GofFile(2000, "eeee");
        yabaiFolder.Add(yabaiFile);
        yabaiFolder.Add(new GofFile(1000, "unn"));

        Folder myFolder = new Folder("my");
        yabaiFolder.Add(myFolder);

        myFolder.Add(new GofFile(500, "test"));

        Console.WriteLine(yabaiFolder.Size);

        TryExecute(() => yabaiFolder.Add(new Folder("my")));
        TryExecute(() => myFolder.Add(new GofFile(500, "test")));
        TryExecute(() => yabaiFile.Add(new GofFile(500, "test")));

        TryExecute(() => myFolder.Remove(myFolder.GetChild(0)));
        TryExecute(() => myFolder.Remove(myFolder.GetChild(0)));
    }
}