using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGL
{
    /// <summary>
    /// An intersection.
    /// </summary>
    public class Intersection
    {
        /// <summary>
        /// Is it intersected?
        /// </summary>
        public bool intersected = false;

        /// <summary>
        /// The normal.
        /// </summary>
        public Vertex normal = new Vertex();

        /// <summary>
        /// The point.
        /// </summary>
        public Vertex point = new Vertex();

        /// <summary>
        /// The closeness.
        /// </summary>
        public float closeness = -1;
    }
}
