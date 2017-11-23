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
        public float Width { get; private set; }
        public float Height { get; private set; }

        public float MinX { get => -Width * 0.5f + PrimitiveView.Position.X; }
        public float MaxX { get => Width * 0.5f + PrimitiveView.Position.X; }

        public float MinY { get => -Height * 0.5f + PrimitiveView.Position.Y; }
        public float MaxY { get => Height * 0.5f + PrimitiveView.Position.Y; }


        public RectangleCollider(float width, float height, Vector2f position) 
        {
            Width = width;
            Height = height;
            Center = new Vector2f(Width * 0.5f, Height * 0.5f);

            SetupVertex(position);
            SetupFaceNormal();
        }

        public RectangleCollider(RectangleView view) : base(view)
        {
            Width = view.Width;
            Height = view.Height;

            SetupVertex(view.Position);
            SetupFaceNormal();
        }


        private void SetupVertex(Vector2f position)
        {
            var halfWidth = Width * 0.5f;
            var halfHeight = Height * 0.5f;

            this.Vertex = new Vector2f[4]
            {
                new Vector2f(-halfWidth + position.X, -halfHeight + position.Y),
                new Vector2f( halfWidth + position.X, -halfHeight + position.Y),
                new Vector2f( halfWidth + position.X,  halfHeight + position.Y),
                new Vector2f(-halfWidth + position.X,  halfHeight + position.Y)
            };
        }

        private void SetupFaceNormal()
        {
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
