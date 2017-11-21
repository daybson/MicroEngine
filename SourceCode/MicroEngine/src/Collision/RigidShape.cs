using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroEngine.src.Collision
{
    public class RigidShape
    {
        public Vector2f[] Vertex { get; private set; }
        public Vector2f[] FaceNormal { get; private set; }
    }
}
