using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace MicroEngine.Entities.Primitives
{
    public class Circle : Primitive
    {
        public float Radius { get; private set; }


        public Circle(float radius, Vector2f center)
        {
            Center = center;
            Radius = radius;

            Shape = new CircleShape(Radius);
            base.ConfigurateShape();
        }
    }
}
