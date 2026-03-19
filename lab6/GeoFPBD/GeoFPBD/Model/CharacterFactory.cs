using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;

namespace GeoFPBD.Model
{
    // FlyweightFactory 
    internal class CharacterFactory
    {
        private Dictionary<string, IRenderable> _characters = new();
        private int _uniqueChars = 0; 
        public Character GetCharacter(char symbol, string font, int fontSize)
        {
            string key = $"{symbol}_{font}_{fontSize}";
            if (!_characters.ContainsKey(key))
            {
                _uniqueChars++;
                Console.WriteLine($"Initializing \t'{symbol}' ({font}, {fontSize}pt)");
                _characters[key] = new Character(symbol, font, fontSize);
            }
            else
            {
                Console.WriteLine($"Reusing \t'{symbol}' ({font}, {fontSize}pt)");
            }
            return (Character)_characters[key];
        }
        public ColorRender GetCharacter(string name, ConsoleColor color)
        {
            _uniqueChars++;
            Console.WriteLine($"Initializing \t'{name}' ({color} color pt)");
            return new ColorRender(name, color);

        }
        public int GetCount() => _uniqueChars;
    }
}
