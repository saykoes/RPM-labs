namespace OOP_Fundamentals_Library
{
    public class Customer
    {
        public string Name;
        public int Age;

        public void PrintInfo()
        {
            Console.WriteLine($"Customer: {Name}, {Age} years old");
        }
    }
}
