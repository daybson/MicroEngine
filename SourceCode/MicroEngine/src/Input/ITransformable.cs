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
        void Move(Vector2f displacement);
        void Rotate(float angle);
    }
}
