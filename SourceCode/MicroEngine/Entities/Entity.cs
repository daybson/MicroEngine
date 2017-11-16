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

        public IMover Mover { get; private set; }
        public Drawable Drawable { get; private set; }

        private bool keyA;
        private bool keyS;
        private bool keyW;
        private bool keyD;

        #endregion


        #region Constructors

        /// <summary>
        /// Constructor that instantiates a new SingleSprite as default Drawable
        /// </summary>
        /// <param name="texturePath">The full path to the texture (path\filename.extension)</param>
        public Entity(string texturePath)
        {
            var singleSprite = new SingleSprite(texturePath);
            Drawable = singleSprite;
            Mover = singleSprite;
        }


        /// <summary>
        /// Constructor that instantiates a new Primitive as default Drawable
        /// </summary>
        /// <param name="primitive">The primitive to be used</param>
        public Entity(Primitive primitive)
        {
            Drawable = primitive;
            Mover = primitive;
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
            }
        }


        public void Update()
        {
            if (keyA)
                Mover.Move(new Vector2f(-1, 0));
            if (keyD)
                Mover.Move(new Vector2f(1, 0));
            if (keyW)
                Mover.Move(new Vector2f(0, -1));
            if (keyS)
                Mover.Move(new Vector2f(0, 1));
        }
    }
}
