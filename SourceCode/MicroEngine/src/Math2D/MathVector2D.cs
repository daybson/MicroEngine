using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroEngine.src.Math2D
{
    public static class MathVector2D
    {
        public static string AsString(this Vector2f v)
        {
            return $"({v.X}, {v.Y})";
        }


        public static float Magnitude(this Vector2f v)
        {
            return (float)Math.Sqrt(v.X * v.X + v.Y * v.Y);
        }


        public static Vector2f Normalize(this Vector2f v)
        {
            var m = v.Magnitude();
            if (m > 0)
                return v / m;
            return v;
        }


        public static float Deg2Rad(float degrees)
        {
            return degrees * 3.1415f / 180f;
        }


        public static float Rad2Deg(float radian)
        {
            return radian * 180f / 3.1415f;
        }


        public static Vector2f Rotate(this Vector2f v, float angle)
        {
            var radians = Deg2Rad(angle);
            var sin = (float)Math.Sin(radians);
            var cos = (float)Math.Cos(radians);

            return new Vector2f(v.X * cos - v.Y * sin,
                                v.X * sin + v.Y * cos);
        }


        public static Vector2f Rotate(this Vector2f v, Vector2f pivot, float degrees)
        {
            float x = v.X - pivot.X;
            float y = v.Y - pivot.Y;

            float radians = Deg2Rad(degrees);
            var sin = (float)Math.Sin(radians);
            var cos = (float)Math.Cos(radians);

            Vector2f r = new Vector2f(x * cos - y * sin,
                                      x * sin + y * cos);

            r.X += pivot.X;
            r.Y += pivot.Y;

            return r;
        }


        public static float Dot(this Vector2f a, Vector2f b)
        {
            return a.X * b.X + a.Y * b.Y;
        }


        public static float EnclosedAngle(this Vector2f a, Vector2f b)
        {
            Vector2f ua = a.Normalize();
            Vector2f ub = b.Normalize();
            float dp = Dot(ua, ub);
            return Rad2Deg((float)Math.Acos(dp));
        }


        public static Vector2f ProjectOnto(this Vector2f project, Vector2f onto)
        {
            float d = Dot(onto, onto);
            if (0 < d)
            {
                float dp = project.Dot(onto);
                return onto * dp / d;
            }
            return onto;
        }


        public static Vector2f Rotate90Deg(this Vector2f v)
        {
            return new Vector2f(-v.Y, v.X);
        }


        public static bool IsParallelTo(this Vector2f a, Vector2f b)
        {
            Vector2f na = a.Rotate90Deg();
            return na.Dot(b) == 0;
        }


        public static float DistanceBetween(this Vector2f a, Vector2f b)
        {
            return (float)Math.Sqrt((b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y));
        }


        public static float RawDistanceBetween(this Vector2f a, Vector2f b)
        {
            return (b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y);
        }


        public static float AngleBetween(this Vector2f a, Vector2f b)
        {
            return (float)(Math.Atan2(b.Y, b.X) - Math.Atan2(a.Y, a.X) / Math.PI * 180f);
        }
    }
}
