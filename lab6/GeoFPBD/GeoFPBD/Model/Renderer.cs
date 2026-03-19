using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoFPBD.Model
{
    public interface IRenderingEngine
    {
        void BeginRender();
        void EndRender();
        void RenderRectangle(float x, float y, float width, float height);
        void RenderEllipse(float x, float y, float radiusX, float radiusY);
        void RenderLine(float x1, float y1, float x2, float y2);
    }

    internal class PrintRenderer : IRenderingEngine
    {
        public void BeginRender() =>
            Console.WriteLine("[Print] Render Start");
        public void EndRender() =>
            Console.WriteLine("[Print] Render End");
        public void RenderRectangle(float x, float y, float width, float height) =>
            Console.WriteLine($"[Print] Rectangle ({x};{y}) {width}x{height}");
        public void RenderEllipse(float x, float y, float radiusX, float radiusY) =>
            Console.WriteLine($"[Print] Ellipse ({x};{y}) radius (rx: {radiusX}; ry:{radiusY})");
        public void RenderLine(float x1, float y1, float x2, float y2) =>
            Console.WriteLine($"[Print] Line ({x1};{y1})--({x2};{y2})");
    }
    public class ScreenRenderer : IRenderingEngine
    {
        public void BeginRender() =>
            Console.WriteLine("[Screen] Render Start");
        public void EndRender() =>
            Console.WriteLine("[Screen] Render End");
        public void RenderRectangle(float x, float y, float width, float height) =>
            Console.WriteLine($"[Screen] Rectangle ({x};{y}) {width}x{height}");
        public void RenderEllipse(float x, float y, float radiusX, float radiusY) =>
            Console.WriteLine($"[Screen] Ellipse ({x};{y}) radius (rx: {radiusX},ry:{radiusY})");
        public void RenderLine(float x1, float y1, float x2, float y2) =>
            Console.WriteLine($"[Screen] Line ({x1};{y1})--({x2};{y2})");
    }
}
