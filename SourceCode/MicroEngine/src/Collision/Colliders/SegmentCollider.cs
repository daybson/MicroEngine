using System;
using System.Collections.Generic;
using System.Text;
using MicroEngine.Entities.Primitives;
using SFML.System;
using MicroEngine.src.Math2D;
using MicroEngine.src.Collision.Helper;

namespace MicroEngine.src.Collision.Colliders
{
    public class SegmentCollider : Collider
    {
        public Vector2f Point1 { get; private set; }
        public Vector2f Point2 { get; private set; }

        public SegmentCollider(Vector2f p1, Vector2f p2)
        {
            Point1 = p1;
            Point2 = p2;
        }

        public SegmentCollider(SegmentView view) : base(view)
        {
            Point1 = view.Point1;
            Point2 = view.Point2;
        }

        public override void Move(Vector2f displacement)
        {
            Point1 += displacement;
            Point2 += displacement;
        }

        public override void Rotate(float angle)
        {
            base.Rotate(angle);

            Point2 = this.Point2.Rotate(this.Point1, angle);
        }

        public Range ProjectOnto(Vector2f onto)
        {
            Vector2f ontoUnit = onto.Normalize();
            return new Range(ontoUnit.Dot(Point1),
                             ontoUnit.Dot(Point2)).Sort();
        }


    }
}
