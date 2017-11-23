using System;
using System.Collections.Generic;
using System.Text;
using MicroEngine.Entities.Primitives;

namespace MicroEngine.src.Collision.Colliders
{
    public class CircleCollider : Collider
    {
        public float Radius { get; private set; }


        public CircleCollider(CircleView view) : base(view)
        {
            Radius = view.Radius;
        }
    }
}
