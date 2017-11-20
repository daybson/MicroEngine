using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace MicroEngine.Entities.Primitives
{
    public class Rectangle : Primitive
    {
        public float Height { get; private set; }
        public float Width { get; private set; }


        public Rectangle(float height, float width, Vector2f position)
        {
            Width = width;
            Height = height;
            Center = new Vector2f(Width * 0.5f, Height * 0.5f);

            Shape = new RectangleShape(new Vector2f(Width, Height));
            Shape.Position = position;
            base.ConfigurateShape();
        }
    }
}
