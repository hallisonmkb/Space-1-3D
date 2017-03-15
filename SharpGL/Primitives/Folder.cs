using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGL
{
    /// <summary>
    /// A Folder is used to organise scene elements.
    /// </summary>
    public class Folder : SceneElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Folder"/> class.
        /// </summary>
        public Folder()
        {
            Name = "Folder";
        }
    }
}
