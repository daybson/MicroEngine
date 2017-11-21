using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace MicroEngine.Entities.Primitives
{
    public class Point : PrimitiveView
    {
        public Point(Vector2f center)
        {
            Center = center;

            Shape = new CircleShape(2);
            base.ConfigurateShape();
        }       
    }
}
