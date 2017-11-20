using MicroEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Window;
using MicroEngine.Core;
using MicroEngine.Entities;
using MicroEngine.Entities.Primitives;

namespace MyGame
{
    public class MainGame : EngineWindow
    {
        public MainGame(string gameName, Vector2u windowSize) : base(gameName, windowSize)
        {
            //this.Game.Entities.Add(new Entity(new Circle(20, new Vector2f(20,20))));
            //this.Game.Entities.Add(new Entity(new Rectangle(20, 50, new Vector2f())));
            //this.Game.Entities.Add(new Entity(new SegmentLine(new Vector2f(), new Vector2f(50, 50))));
            this.Game.Entities.Add(new Entity(@"D:\Repositorio\MicroEngine\SourceCode\MicroEngine\res\textures\tri.png", new Vector2f(0, 0)));

            Run();
        }
    }
}
