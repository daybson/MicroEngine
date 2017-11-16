using MicroEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Window;
using MicroEngine.Core;

namespace MyGame
{
    public class MainGame : EngineWindow
    {
        public MainGame(string gameName, Vector2u windowSize) : base(gameName, windowSize)
        {
            Run();
        }
    }
}
