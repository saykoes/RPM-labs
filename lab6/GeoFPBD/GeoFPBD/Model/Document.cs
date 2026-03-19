using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoFPBD.Model
{
    public class Document(IRenderingEngine engine)
    {
        private List<Page> _pages = new List<Page>();
        public Page CreatePage()
        {
            var page = new Page();
            _pages.Add(page);
            return page;
        }
        public void RenderAll()
        {
            engine.BeginRender();
            for (int i = 0; i < _pages.Count; i++)
            {
                Console.WriteLine($"\n--- Page {i + 1} ---");
                _pages[i].Render();
            }
            engine.EndRender();
        }
    }
    public class Page
    {
        private List<IDrawable> _drawables = new List<IDrawable>();
        public void Add(IDrawable drawable)
        {
            _drawables.Add(drawable);
        }
        public void Render()
        {
            foreach (var d in _drawables)
            {
                d.Draw();
                Console.WriteLine();
            }
        }
    }
}
