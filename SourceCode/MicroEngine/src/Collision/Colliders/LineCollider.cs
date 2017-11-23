using System;
using System.Collections.Generic;
using System.Text;
using MicroEngine.Entities.Primitives;
using SFML.System;
using MicroEngine.src.Math2D;

namespace MicroEngine.src.Collision.Colliders
{
    public class LineCollider : Collider
    {
        public Vector2f Base { get; private set; }
        public Vector2f Direction { get; private set; }
        public Vector2f SecondPoint { get; private set; }


        private void CalculateDirection() => Direction = Base - SecondPoint;


        public LineCollider(Vector2f p1, Vector2f p2)
        {
            Base = p1;
            SecondPoint = p2;
            CalculateDirection();
        }


        public LineCollider(SegmentView view) : base(view)
        {
            Base = view.Point1;
            SecondPoint = view.Point2;
            CalculateDirection();
        }


        public override void Move(Vector2f displacement)
        {
            base.Move(displacement);

            Base += displacement;
            SecondPoint += displacement;
            CalculateDirection();
        }


        public override void Rotate(float angle)
        {
            base.Rotate(angle);
            SecondPoint = SecondPoint.Rotate(Base, angle);
            CalculateDirection();
        }

        public bool IsEquivalent(LineCollider b)
        {
            if (!Direction.IsParallelTo(b.Direction))
                return false;
            Vector2f d = Base - b.Base;
            return d.IsParallelTo(Direction);
        }


        public Vector2f GetPointOnLine(float scalar)
        {
            /** r = r0 + td
             * r0 = ponto base
             * t = escalar
             * d = direção
             * */
            return this.Base + (scalar * this.Direction);
        }


        public bool IsOnOneSide(SegmentCollider s)
        {
            Vector2f d1 = s.Point1 - Base;
            Vector2f d2 = s.Point2 - Base;
            Vector2f n = Direction.Rotate90Deg();
            return n.Dot(d1) * n.Dot(d2) > 0;
        }
    }
}
