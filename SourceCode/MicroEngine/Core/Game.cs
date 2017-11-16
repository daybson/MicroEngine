using MicroEngine.Entities;
using MicroEngine.Entities.Primitives;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroEngine.Core
{
    public class Game
    {
        public bool IsRuning { get; set; }
        public List<Entity> Entities = new List<Entity>();

        public Game()
        {
            //Entities.Add(new Entity(new Circle(20, new Vector2f())));
            //Entities.Add(new Entity(new Rectangle(20, 50, new Vector2f())));
            Entities.Add(new Entity(new SegmentLine(new Vector2f(), new Vector2f(50,50))));
            //Entities.Add(new Entity(@"D:\Repositorio\MicroEngine\SourceCode\MicroEngine\res\textures\tri.png"));
            IsRuning = true;
        }


        public void Update()
        {
            Entities.ForEach(e => e.Update());
        }
    }
}
