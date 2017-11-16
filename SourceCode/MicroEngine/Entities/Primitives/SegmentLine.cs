using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroEngine.Entities.Primitives
{
    public class SegmentLine : Primitive
    {
        public Vector2f Point1 { get; private set; }
        public Vector2f Point2 { get; private set; }
        protected Vertex[] vertex;


        /// <summary>
        /// Instantiate a new SegmentLine render object
        /// </summary>
        /// <param name="point1">Start point</param>
        /// <param name="point2">End point</param>
        public SegmentLine(Vector2f point1, Vector2f point2)
        {
            Point1 = point1;
            Point2 = point2;
            Center = Point2 - Point1;

            this.vertex = new Vertex[2]
            {
                new Vertex(Point1, this.ShapeOutlineColor),
                new Vertex(Point2, this.ShapeOutlineColor)
            };
        }


        /// <summary>
        /// Tells some RenderTarget (usually the RenderWindow) to draws the line segment composed by an vertex array
        /// </summary>
        /// <param name="renderTarget">The render which is used to draw</param>
        public override void Draw(RenderTarget renderTarget, RenderStates renderStates)
        {
            renderTarget.Draw(this.vertex, PrimitiveType.Lines);
        }


        public override void Move(Vector2f displacement)
        {
            Point1 += displacement;
            Point2 += displacement;
            Center = Point2 - Point1;

            this.vertex = new Vertex[2]
            {
                new Vertex(Point1, this.ShapeOutlineColor),
                new Vertex(Point2, this.ShapeOutlineColor)
            };
        }
    }
}
