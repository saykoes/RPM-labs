using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp
{
    public interface IGeoFactory
    {
        Circle CreateCircle();
        Square CreateSquare();
        Triangle CreateTriangle();
    }

    public class RedFactory : IGeoFactory
    {
        public Circle CreateCircle() => new Circle { Color = Colors.Red };
        public Square CreateSquare() => new Square { Color = Colors.Red };
        public Triangle CreateTriangle() => new Triangle { Color = Colors.Red };
        
    }
    public class GreenFactory : IGeoFactory
    {
        public Circle CreateCircle() => new Circle { Color = Colors.Green };
        public Square CreateSquare() => new Square { Color = Colors.Green };
        public Triangle CreateTriangle() => new Triangle { Color = Colors.Green };
    }
    public class BlueFactory : IGeoFactory
    {
        public Circle CreateCircle() => new Circle { Color = Colors.Blue };
        public Square CreateSquare() => new Square { Color = Colors.Blue };
        public Triangle CreateTriangle() => new Triangle { Color = Colors.Blue };
    }
    public class CyanFactory : IGeoFactory
    {
        public Circle CreateCircle() => new Circle { Color = Colors.Cyan };
        public Square CreateSquare() => new Square { Color = Colors.Cyan };
        public Triangle CreateTriangle() => new Triangle { Color = Colors.Cyan };
    }
    public class MagentaFactory : IGeoFactory
    {
        public Circle CreateCircle() => new Circle { Color = Colors.Magenta };
        public Square CreateSquare() => new Square { Color = Colors.Magenta };
        public Triangle CreateTriangle() => new Triangle { Color = Colors.Magenta };
    }
    public class YellowFactory : IGeoFactory
    {
        public Circle CreateCircle() => new Circle { Color = Colors.Yellow };
        public Square CreateSquare() => new Square { Color = Colors.Yellow };
        public Triangle CreateTriangle() => new Triangle { Color = Colors.Yellow };
    }
    public class BlackFactory : IGeoFactory
    {
        public Circle CreateCircle() => new Circle { Color = Colors.Black };
        public Square CreateSquare() => new Square { Color = Colors.Black };
        public Triangle CreateTriangle() => new Triangle { Color = Colors.Black };
    }
}
