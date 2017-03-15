using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Space1
{
    public class Cliente
    {
        public string Descricao { get; set; }
        public Color ForeColor { get; set; }
        public PedidoCollection PedidoTodos { get; set; }
    }
}
