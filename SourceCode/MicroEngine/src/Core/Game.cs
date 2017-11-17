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
            IsRuning = true;
        }


        public void Update()
        {
            Entities.ForEach(e => e.Update());
        }
    }
}
