using System;
using System.ComponentModel;

namespace SharpGL
{
    /// <summary>
    /// The Quadric orientation.
    /// </summary>
    public enum Orientation : uint
    {
        /// <summary>
        /// Outside.
        /// </summary>
        Outside = OpenGL.GLU_OUTSIDE,

        /// <summary>
        /// Inside.
        /// </summary>
        Inside = OpenGL.GLU_INSIDE,
    }
}