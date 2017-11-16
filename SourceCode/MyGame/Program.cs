using MicroEngine;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    class Program
    {
        static void Main(string[] args)
        {
            new MainGame("My Game", new Vector2u(600, 400));
        }
    }
}
