using System;
using System.Collections.Generic;
using System.Text;
using MicroEngine.Entities.Primitives;
using MicroEngine.src.Math2D;
using SFML.System;

namespace MicroEngine.src.Collision.Colliders
{
    public class RectangleCollider : Collider
    {
        public RectangleCollider(RectangleView view) : base(view)
        {
            var halfWidth = view.Width * 0.5f;
            var halfHeight = view.Height * 0.5f;

            this.Vertex = new Vector2f[4]
            {
                new Vector2f(-halfWidth, -halfHeight),
                new Vector2f(halfWidth,  -halfHeight),
                new Vector2f(halfWidth,   halfHeight),
                new Vector2f(-halfWidth,  halfHeight)
            };

            this.FaceNormal = new Vector2f[4]
            {
                (this.Vertex[1] - this.Vertex[2]).Normalize(),
                (this.Vertex[2] - this.Vertex[3]).Normalize(),
                (this.Vertex[3] - this.Vertex[0]).Normalize(),
                (this.Vertex[0] - this.Vertex[1]).Normalize(),
            };
        }

        public override void Rotate(float angle)
        {
            base.Rotate(angle);

            for (int i = 0; i < this.Vertex.Length; i++)
                this.Vertex[i] = this.Vertex[i].Rotate(this.Center, angle);

            this.FaceNormal[0] = (this.Vertex[1] - this.Vertex[2]).Normalize();
            this.FaceNormal[1] = (this.Vertex[2] - this.Vertex[3]).Normalize();
            this.FaceNormal[2] = (this.Vertex[3] - this.Vertex[0]).Normalize();
            this.FaceNormal[3] = (this.Vertex[0] - this.Vertex[1]).Normalize();
        }
    }
}
