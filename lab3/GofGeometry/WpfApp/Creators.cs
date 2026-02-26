using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp
{
    /// <summary>
    /// Circles
    /// </summary>

    public abstract class CircleCreator
    {
        public abstract Circle CreateCircle();
    }

    public class RedCircleCreator : CircleCreator
    {
        public override Circle CreateCircle() => new Circle { Color = Colors.Red };
    }

    public class GreenCircleCreator : CircleCreator
    {
        public override Circle CreateCircle() => new Circle { Color = Colors.Green };
    }
    public class BlueCircleCreator : CircleCreator
    {
        public override Circle CreateCircle() => new Circle { Color = Colors.Blue };
    }
    public class CyanCircleCreator : CircleCreator
    {
        public override Circle CreateCircle() => new Circle { Color = Colors.Cyan };
    }
    public class MagentaCircleCreator : CircleCreator
    {
        public override Circle CreateCircle() => new Circle { Color = Colors.Magenta };
    }
    public class YellowCircleCreator : CircleCreator
    {
        public override Circle CreateCircle() => new Circle { Color = Colors.Yellow };
    }
    public class BlackCircleCreator : CircleCreator
    {
        public override Circle CreateCircle() => new Circle { Color = Colors.Black };
    }

    /// <summary>
    /// Squares
    /// </summary>

    public abstract class SquareCreator
    {
        public abstract Square CreateSquare();
    }
    public class RedSquareCreator : SquareCreator
    {
        public override Square CreateSquare() => new Square { Color = Colors.Red };
    }
    public class GreenSquareCreator : SquareCreator
    {
        public override Square CreateSquare() => new Square { Color = Colors.Green };
    }
    public class BlueSquareCreator : SquareCreator
    {
        public override Square CreateSquare() => new Square { Color = Colors.Blue };
    }
    public class CyanSquareCreator : SquareCreator
    {
        public override Square CreateSquare() => new Square { Color = Colors.Cyan };
    }
    public class MagentaSquareCreator : SquareCreator
    {
        public override Square CreateSquare() => new Square { Color = Colors.Magenta };
    }
    public class YellowSquareCreator : SquareCreator
    {
        public override Square CreateSquare() => new Square { Color = Colors.Yellow };
    }
    public class BlackSquareCreator : SquareCreator
    {
        public override Square CreateSquare() => new Square { Color = Colors.Black };
    }

    /// <summary>
    /// Traingles
    /// </summary>

    public abstract class TriangleCreator
    {
        public abstract Triangle CreateTriangle();
    }

    public class RedTriangleCreator : TriangleCreator
    {
        public override Triangle CreateTriangle() => new Triangle { Color = Colors.Red };
    }
    public class GreenTriangleCreator : TriangleCreator
    {
        public override Triangle CreateTriangle() => new Triangle { Color = Colors.Green };
    }
    public class BlueTriangleCreator : TriangleCreator
    {
        public override Triangle CreateTriangle() => new Triangle { Color = Colors.Blue };
    }
    public class CyanTriangleCreator : TriangleCreator
    {
        public override Triangle CreateTriangle() => new Triangle { Color = Colors.Cyan };
    }
    public class MagentaTriangleCreator : TriangleCreator
    {
        public override Triangle CreateTriangle() => new Triangle { Color = Colors.Magenta };
    }
    public class YellowTriangleCreator : TriangleCreator
    {
        public override Triangle CreateTriangle() => new Triangle { Color = Colors.Yellow };
    }
    public class BlackTriangleCreator : TriangleCreator
    {
        public override Triangle CreateTriangle() => new Triangle { Color = Colors.Black };
    }
}
