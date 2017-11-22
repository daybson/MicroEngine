using System;
using System.Collections.Generic;
using System.Text;
using MicroEngine.Entities.Primitives;
using SFML.System;
using MicroEngine.src.Math2D;

namespace MicroEngine.src.Collision
{
    public class LineCollider : Collider
    {
        public Vector2f Base { get; private set; }
        public Vector2f Direction { get; private set; }
        private Vector2f secondPoint;

        public LineCollider(SegmentView view) : base(view)
        {
            Base = view.Point1;
            secondPoint = view.Point2;
            CalculateDirection();
        }

        public override void Move(Vector2f displacement)
        {
            base.Move(displacement);

            Base += displacement;
            secondPoint += displacement;
            CalculateDirection();
        }

        public override void Rotate(float angle)
        {
            base.Rotate(angle);
            secondPoint = secondPoint.Rotate(Base, angle);
            CalculateDirection();
        }

        private void CalculateDirection() => Direction = Base - secondPoint;
    }
}
