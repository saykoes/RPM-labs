using GeoFPBD.Model;
using System.Data.Common;
using System.Security.Cryptography.X509Certificates;

// Client
internal class Program
{
    // Record for characters with position (Extrinsic state will be stored with it)
    public record posChar (int posX, int posY, IRenderable CharObj);
    
    private static void Main(string[] args)
    { 
        // Flyweight
        CharacterFactory charFactory = new CharacterFactory();

        var screenChars = new List<posChar>
        {
            new posChar(5, 5, charFactory.GetCharacter('T', "Verdana", 16)),
            new posChar(100, 100, charFactory.GetCharacter('D', "Courier New", 14)),
            new posChar(50, 10, charFactory.GetCharacter('W', "Georgia", 18)),
            new posChar(15, 5, charFactory.GetCharacter('T', "Verdana", 16)),  // Same as first char
            new posChar(0, 200, charFactory.GetCharacter('P', "Impact", 20)),
            new posChar(110, 100, charFactory.GetCharacter('D', "Courier New", 14)), // Same as second char
            new posChar(11, 11, charFactory.GetCharacter("Color is cool!", ConsoleColor.Green)), // Custom
        };

        Console.WriteLine();

        foreach (var curChar in screenChars) 
            curChar.CharObj.Render(curChar.posX,curChar.posY);

        Console.WriteLine();

        Console.WriteLine($"Unique characters: {charFactory.GetCount()}");

        // Proxy

        Console.WriteLine();

        // (will not load imgs)
        IImage img1 = new ImageProxy("image1.png");
        IImage img2 = new ImageProxy("image2.png");
        IImage img3 = new ImageProxy("image3.png");
        
        Console.WriteLine();

        Console.WriteLine($"img1 Width: {img1.GetWidth()}px"); // will load img1
        img2.Render(34,56); // will load img2
        Console.WriteLine($"img1 Heigth: {img1.GetHeight()}px"); // already loaded
        
        // img3 won't be loaded
    }
}