using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SharpGL;

namespace Space1
{
    public class Pedido
    {
        public string Descricao { get; set; }
        public List<SceneElement> ElementoTodos { get; set; }
    }
}
