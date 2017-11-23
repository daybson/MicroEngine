using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroEngine.src.Collision
{
    /// <summary>
    /// Class to store the collision's response
    /// </summary>
    public class CollisionInfo
    {

        /// <summary>
        /// The Minimum Translation Distance
        /// </summary>
        public float MTD { get; private set; }

        /// <summary>
        /// The collision orientation
        /// </summary>
        public Vector2f Normal { get; private set; }

        /// <summary>
        /// The collision's start point 
        /// </summary>
        public Vector2f Start { get; private set; }

        /// <summary>
        /// The collision's end point 
        /// </summary>
        public Vector2f End { get => Start + Normal * MTD; }


        public CollisionInfo(Vector2f start, Vector2f normal, float mtd)
        {
            MTD = Math.Abs(mtd);
            Normal = normal;
            Start = start;
        }


        public Vector2f FlipNormal()
        {
            Normal *= -1;
            return Normal;
        }
    }
}
