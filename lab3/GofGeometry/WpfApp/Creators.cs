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

    /// <summary>
    /// Squares
    /// </summary>

    public abstract class SquareCreator
    {
        public abstract Square CreateSquare();
    }

    /// <summary>
    /// Traingles
    /// </summary>

    public abstract class TriangleCreator
    {
        public abstract Triangle CreateTriangle();
    }
}
