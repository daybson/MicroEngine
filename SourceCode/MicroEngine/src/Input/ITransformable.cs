using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroEngine.Input
{
    /// <summary>
    /// Interface to expose movement methods
    /// </summary>
    public interface ITransformable
    {
        Vector2f Position { get; }
        Vector2f Center { get; }
        float Rotation { get; }

        void Move(Vector2f displacement);
        void Rotate(float angle);
    }
}
