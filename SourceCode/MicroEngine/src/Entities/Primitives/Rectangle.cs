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


        public Rectangle(float height, float width, Vector2f center)
        {
            Height = height;
            Width = width;
            Center = center;

            Shape = new RectangleShape(new Vector2f(Width, Height));
            base.ConfigurateShape();
        }
    }
}
