using MicroEngine.src.Math2D;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroEngine.src.Collision.Helper
{
    public struct Range
    {
        public float Min { get; private set; }
        public float Max { get; private set; }


        public Range(float minimum, float maximum)
        {
            Min = minimum;
            Max = maximum;
        }


        public Range Sort()
        {
            if (Min > Max)
            {
                var t = Max;
                Max = Min;
                Min = t;
            }

            return this;
        }


        public Range GetHull(Range b)
        {
            return new Range(Min < b.Min ? Min : b.Min,
                             Max > b.Max ? Max : b.Max);
        }


        public bool IsOverlapping(Range b)
        {
            return GameMath.InclusiveOverlaps(Min, Max, b.Min, b.Max);
        }
    }
}
