using MicroEngine.Input;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroEngine.Entities.Primitives
{
    /// <summary>
    /// Base class for primitive types
    /// </summary>
    public abstract class Primitive : Drawable, IMover
    {
        public Shape Shape { get; protected set; }
        public Vector2f Center { get; protected set; }
        public Color ShapeOutlineColor { get { return Color.Green; } }
        public float ShapeOutlineThickness { get { return 1f; } }
        public Color ShapeFillColor { get { return Color.White; } }


        protected virtual void ConfigurateShape()
        {
            if (Shape != null)
            {
                Shape.OutlineColor = ShapeOutlineColor;
                Shape.OutlineThickness = ShapeOutlineThickness;
                Shape.FillColor = ShapeFillColor;
                Shape.Position = Center;
            }
        }


        /// <summary>
        /// Method used to draw the primitive on some RenderTarget (usually the RenderWindow)
        /// </summary>
        /// <param name="renderTarget">Where to render the primitive</param>
        public virtual void Draw(RenderTarget renderTarget, RenderStates states)
        {
            if (Shape != null)
                renderTarget.Draw(Shape);
        }

        public virtual void Move(Vector2f displacement)
        {
            if (Shape != null)
                Shape.Position += displacement;
        }
    }
}