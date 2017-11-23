using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroEngine.src.Collision.Helper
{
    public struct SupportStruct
    {
        public Vector2f Point;
        public float Distance;


        public SupportStruct(Vector2f point, float distance)
        {
            this.Point = point;
            this.Distance = distance;
        }
    };
}
