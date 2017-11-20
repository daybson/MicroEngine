using MicroEngine.Input;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace MicroEngine.Entities
{
    /// <summary>
    /// Custom class to handle a single sprite
    /// </summary>
    public class SingleSprite : Drawable, ITransformable
    {
        protected Texture texture;
        protected Sprite Sprite { get; set; }

        public SingleSprite(string texturePath, Vector2f position)
        {
            this.texture = new Texture(texturePath);
            Sprite = new Sprite(this.texture);
            Sprite.Origin = (Vector2f) this.texture.Size * 0.5f;
            Sprite.Position += position;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(Sprite);
        }

        public void Move(Vector2f displacement)
        {
            Sprite.Position += displacement;
        }

        public void Rotate(float angle)
        {
            Sprite.Rotation += angle;
        }
    }
}
