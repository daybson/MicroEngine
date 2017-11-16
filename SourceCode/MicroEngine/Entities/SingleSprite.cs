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
    public class SingleSprite : Drawable, IMover
    {
        protected Texture texture;
        protected Sprite Sprite { get; set; }

        public SingleSprite(string texturePath)
        {
            this.texture = new Texture(texturePath);
            Sprite = new Sprite(this.texture);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(Sprite);
        }

        public void Move(Vector2f displacement)
        {
            Sprite.Position += displacement;
        }
    }
}
