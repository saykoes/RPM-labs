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
    }
}