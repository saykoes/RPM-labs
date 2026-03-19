using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoFPBD.Model
{
    // Single interface for Images and Proxies of Images
    public interface IImage : IRenderable
    {
        int GetWidth();
        int GetHeight();
    }
    // Actual object (let's imagine it's large in size)
    public class HighResolutionImage : IImage
    {
        private string _filename;
        private int _width;
        private int _height;
        public HighResolutionImage(string filename)
        {
            _filename = filename;
            Console.Write($"Loading {_filename}... ");
            LoadFromDisk();
        }
        private void LoadFromDisk()
        {
            // Slow loading imitation
            Thread.Sleep(1000);
            _width = 1920;
            _height = 1080;
            Console.WriteLine($"loaded ({_width}x{_height})");
        }
        public void Render(int posX, int posY)
        {
            Console.WriteLine($"Rendering image '{_filename}' at ({posX},{posY})");
        }
        public int GetWidth() => _width;
        public int GetHeight() => _height;
    }
    // Proxy 
    public class ImageProxy : IImage
    {
        private string _filename;
        private HighResolutionImage? _realImage;
        public ImageProxy(string filename)
        {
            _filename = filename;
            Console.WriteLine($"[Proxy] Craeted proxy for {_filename}");
        }
        private void EnsureLoaded()
        {
            if (_realImage == null)
            {
                Console.WriteLine($"[Proxy] Method is called: Loading object");
                _realImage = new HighResolutionImage(_filename);
            }
        }
        public void Render(int posX, int posY)
        {
            EnsureLoaded();
            Console.Write($"[Proxy]");
            _realImage?.Render(posX, posY);
        }
        public int GetWidth()
        {
            EnsureLoaded();
            Console.WriteLine($"[Proxy]");
            return _realImage?.GetWidth() ?? 0;
        }
        public int GetHeight()
        {
            EnsureLoaded();
            Console.WriteLine($"[Proxy]");
            return _realImage?.GetHeight() ?? 0;
        }
    }
}
