using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace MicroEngine.Entities.Primitives
{
    public class Circle : PrimitiveView
    {
        public float Radius { get; private set; }


        public Circle(float radius, Vector2f position)
        {
            Radius = radius;
            Center = new Vector2f(radius, radius);

            Shape = new CircleShape(Radius);
            Shape.Position = position;
            base.ConfigurateShape();
        }
    }
}
