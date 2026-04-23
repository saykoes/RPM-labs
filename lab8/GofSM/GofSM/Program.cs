using System.Reflection.Metadata;

namespace GofSM
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("New job: ");

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
        }
    }
}
