using MicroEngine.Entities.Primitives;
using MicroEngine.Input;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroEngine.src.Collision
{
    public abstract class Collider : ITransformable
    {
        public Vector2f[] Vertex { get; protected set; }
        public Vector2f[] FaceNormal { get; protected set; }
        public PrimitiveView PrimitiveView { get; protected set; }

        public Vector2f Position { get => PrimitiveView.Position; set => PrimitiveView.Position = value; }
        public float Rotation { get => PrimitiveView.Rotation; set => PrimitiveView.Rotation = value; }
        public Vector2f Center { get => PrimitiveView.Center; set => PrimitiveView.Center = value; }


        public Collider(PrimitiveView primitiveView)
        {
            PrimitiveView = primitiveView;
        }


        public virtual void Move(Vector2f displacement)
        {
            Center += displacement;

            if (Vertex != null)
            {
                for (int i = 0; i < this.Vertex.Length; i++)
                    this.Vertex[i] += displacement;
            }
        }


        public virtual void Rotate(float angle)
        {
            Rotation += angle;
        }
    }
}
