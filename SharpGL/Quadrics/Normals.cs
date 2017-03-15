using System;
using System.ComponentModel;

namespace SharpGL
{
    /// <summary>
    /// The Quadric Normals.
    /// </summary>
    public enum Normals : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = OpenGL.GLU_NONE,

        /// <summary>
        /// Flat.
        /// </summary>
        Flat = OpenGL.GLU_FLAT,

        /// <summary>
        /// Smooth.
        /// </summary>
        Smooth = OpenGL.GLU_SMOOTH
    }
}