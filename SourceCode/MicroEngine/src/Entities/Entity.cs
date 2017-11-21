using MicroEngine.Entities.Primitives;
using MicroEngine.Input;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SFML.Window.Keyboard;

namespace MicroEngine.Entities
{
    /// <summary>
    /// Classe that represents an rendered and controllable object inside the game
    /// </summary>
    public class Entity
    {
        #region Fields

        public ITransformable Transformable { get; private set; }
        public Drawable Drawable { get; private set; }

        private bool keyA;
        private bool keyS;
        private bool keyW;
        private bool keyD;
        private bool keyR;

        #endregion


        #region Constructors

        /// <summary>
        /// Constructor that instantiates a new SingleSprite as default Drawable
        /// </summary>
        /// <param name="texturePath">The full path to the texture (path\filename.extension)</param>
        /// <param name="position">The entity's position </param>
        public Entity(string texturePath, Vector2f position)
        {
            var singleSprite = new SingleSprite(texturePath, position);
            Drawable = singleSprite;
            Transformable = singleSprite;
        }


        /// <summary>
        /// Constructor that instantiates a new Primitive as default Drawable
        /// </summary>
        /// <param name="primitive">The primitive to be used</param>
        public Entity(PrimitiveView primitive)
        {
            Drawable = primitive;
            Transformable = primitive;
        }
        
        #endregion




        public void ReceiveInput(Key key)
        {
            switch (key)
            {
                case Key.A:
                    keyA = true;
                    break;
                case Key.D:
                    keyD = true;
                    break;
                case Key.W:
                    keyW = true;
                    break;
                case Key.S:
                    keyS = true;
                    break;
                case Key.R:
                    keyR = true;
                    break;
            }
        }


        public void ReleaseInput(Key key)
        {
            switch (key)
            {
                case Key.A:
                    keyA = false;
                    break;
                case Key.D:
                    keyD = false;
                    break;
                case Key.W:
                    keyW = false;
                    break;
                case Key.S:
                    keyS = false;
                    break;
                case Key.R:
                    keyR = false;
                    break;
            }
        }


        public void Update()
        {
            if (keyA)
                Transformable.Move(new Vector2f(-1, 0));
            if (keyD)
                Transformable.Move(new Vector2f(1, 0));
            if (keyW)
                Transformable.Move(new Vector2f(0, -1));
            if (keyS)
                Transformable.Move(new Vector2f(0, 1));
            if (keyR)
                Transformable.Rotate(1);
        }
    }
}
