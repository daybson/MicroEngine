using MicroEngine.Core;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroEngine.Core
{
    /// <summary>
    /// Base class for games
    /// </summary>
    public class EngineWindow
    {
        #region Fields

        protected string title;
        protected Vector2u size;
        protected RenderWindow renderWindow;
        public Game Game;

        #endregion


        /// <summary>
        /// Instantiate a new game and its window
        /// </summary>
        /// <param name="title">The name to display on title bar</param>
        /// <param name="size">The window's size</param>
        public EngineWindow(string title, Vector2u size)
        {
            this.title = title;
            this.size = size;

            //Create a new window, default style (minimize, maximize, close)
            this.renderWindow = new RenderWindow(new VideoMode(size.X, size.Y), this.title, Styles.Default);

            //Set the name to display on title bar (for some reason, pass the name in the RenderWindow constructor is not enough)
            this.renderWindow.SetTitle(title);

            //We need to assign an close event to tell the window what to do when user hits the 'close' button
            this.renderWindow.Closed += RenderWindowClosed; ;

            Game = new Game();
        }


        #region RenderWindow

        /// <summary>
        /// Event called when the renderWindow is closed. It free resources and then actually closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenderWindowClosed(object sender, EventArgs e)
        {
            FreeResources();
            this.renderWindow.Close();
        }

        #endregion




        /// <summary>
        /// The GameLoop method. Runs while the window is opened.
        /// </summary>
        public void Run()
        {
            //When instantiated, the window will be opened. So the loop will run whilst window is open
            while (this.renderWindow.IsOpen)
            {
                Console.WriteLine("Window is open");

                //First check for all assinged window events and inputs (keyboad, mouse, joystick)
                this.renderWindow.DispatchEvents();

                //If losts the focus, stop the game update loop but keep waiting the window regain the focus or be closed
                if (!this.renderWindow.HasFocus())
                {
                    Console.WriteLine("Window is not focused");
                    Game.IsRuning = false;
                    continue;
                }

                Console.WriteLine("Window is focused");

                if (Game.IsRuning)
                {
                    //... update entities (input and state)
                    //... render
                }

                //Every frame, we clear the render buffer with a color
                this.renderWindow.Clear(Color.Blue);

                //This method actualy draws on the screen.
                this.renderWindow.Display();
            }
        }



        /// <summary>
        /// Unload all resources
        /// </summary>
        private void FreeResources()
        {
            Game.IsRuning = false;
            Console.WriteLine("Window is closed");
        }
    }
}