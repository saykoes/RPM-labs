using GeoFPBD.Model;
using System;
using System.Data.Common;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using Document = GeoFPBD.Model.Document;

// Client
internal class Program
{
    // Record for characters with position (Extrinsic state will be stored with it)
    public record posChar (int posX, int posY, IRenderable CharObj);
    
    private static void Main(string[] args)
    { 
        // --- Flyweight ---
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

        // --- Bridge ---

        Console.WriteLine();

        IRenderingEngine screenEngine = new ScreenRenderer();
        IRenderingEngine printEngine = new PrintRenderer();
        
        // printEngine
        Rectangle printRect = new Rectangle(printEngine, 15.5f, 30.0f, 120.0f, 60.5f);
        Ellipse printEllipse = new Ellipse(printEngine, 250.0f, 150.0f, 45.0f, 45.0f);
        Line printLine = new Line(printEngine, 10.0f, 10.0f, 300.0f, 450.0f);

        printEngine.BeginRender();
        printRect.Draw();
        printEllipse.Draw();
        printLine.Draw();
        printEngine.EndRender();

        Console.WriteLine();

        // screenEngine
        Rectangle screenRect = new Rectangle(screenEngine, 15.5f, 30.0f, 120.0f, 60.5f);
        Ellipse screenEllipse = new Ellipse(screenEngine, 250.0f, 150.0f, 45.0f, 45.0f);
        Line screenLine = new Line(screenEngine, 10.0f, 10.0f, 300.0f, 450.0f);

        screenEngine.BeginRender();
        screenRect.Draw();
        screenEllipse.Draw();
        screenLine.Draw();
        screenEngine.EndRender();

        Console.WriteLine();

        printLine.Move(5.0f, 10.0f);
        screenEngine.BeginRender();
        printLine.Draw();
        screenEngine.EndRender();

        // --- Decorator ---

        Console.WriteLine("\n--- Decorator ---\n");

        IDrawable neonRect = new ShadowDecorator(screenRect, 12);
        IDrawable thickBorderEllipse = new BorderDecorator(screenEllipse, 8);
        IDrawable ghostLine = new TransparencyDecorator(screenLine, 85);

        neonRect.Draw(); Console.WriteLine();
        thickBorderEllipse.Draw(); Console.WriteLine();
        ghostLine.Draw(); Console.WriteLine();

        Console.WriteLine();

        IDrawable glassRect = new BorderDecorator(
                                new TransparencyDecorator(screenRect, 40), 2);

        IDrawable heavyEllipse = new BorderDecorator(
                                    new ShadowDecorator(screenEllipse, 15), 4);

        IDrawable complexLine = new BorderDecorator(
                                    new TransparencyDecorator(
                                        new ShadowDecorator(screenLine, 6), 25), 1);

        glassRect.Draw(); Console.WriteLine();
        heavyEllipse.Draw(); Console.WriteLine();
        complexLine.Draw(); Console.WriteLine();

        Console.WriteLine();

        Document mainDoc = new Document(screenEngine);

        Page coverPage = mainDoc.CreatePage(); 
        coverPage.Add(screenEllipse);
        coverPage.Add(neonRect);
        coverPage.Add(heavyEllipse);

        Page contentPage = mainDoc.CreatePage();
        contentPage.Add(screenLine);
        contentPage.Add(ghostLine);
        contentPage.Add(glassRect);
        contentPage.Add(complexLine);

        mainDoc.RenderAll();
    }
}