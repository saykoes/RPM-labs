using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GeoFPBD.Model
{
    // Flyweight 
    public interface IRenderable
    {
        void Render(int line, int col);
    }
    // ConcreteFlyweight (Intrinsic state will be stored with it)
    internal class Character(char symbol, string font, int fontSize) : IRenderable
    {
        public void Render(int positionX, int positionY)
        {
            Console.WriteLine($"Rendered '{symbol}'({font}, {fontSize}pt)\tat ({positionX}, {positionY})");
        }
    }
    // UnsharedConcreteFlyweight (Intrinsic state will be stored with it)
    public class ColorRender(string name, ConsoleColor color) : IRenderable
    {
        public void Render(int line, int column)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"Custom Render '{name}'\tat ({line}, {column})");
            Console.ResetColor();
        }
    }
}
