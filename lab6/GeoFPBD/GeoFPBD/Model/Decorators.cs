using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoFPBD.Model
{
    public abstract class DrawableDecorator : IDrawable
    {
        protected IDrawable _wrappee;
        public DrawableDecorator(IDrawable wrappee)
        {
            _wrappee = wrappee;
        }
        public virtual void Draw()
        {
            _wrappee.Draw();
        }
    }

    internal class BorderDecorator : DrawableDecorator
    {
        private int _borderWidth;
        public BorderDecorator(IDrawable wrappee, int borderWidth) : base(wrappee)
        {
            _borderWidth = borderWidth;
        }
        public override void Draw()
        {
            base.Draw();
            Console.WriteLine($"|---[Border (Width: {_borderWidth}px)]");
        }
    }
    internal class ShadowDecorator : DrawableDecorator
    {
        private int _shadowOffset;
        public ShadowDecorator(IDrawable wrappee, int shadowOffset) : base(wrappee)
        {
            _shadowOffset = shadowOffset;
        }
        public override void Draw()
        {
            base.Draw();
            Console.WriteLine($"|---[Shadow (Offset: {_shadowOffset}px)]");
        }
    }
    internal class TransparencyDecorator : DrawableDecorator
    {
        private int _transparencyLevel;
        public TransparencyDecorator(IDrawable wrappee, int transparencyLevel) : base(wrappee)
        {
            _transparencyLevel = transparencyLevel;
        }
        public override void Draw()
        {
            base.Draw();
            Console.WriteLine($"|---[Transpareny ({_transparencyLevel}%)]");
        }
    }
}
