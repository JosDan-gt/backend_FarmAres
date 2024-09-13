using System;
using System.Collections.Generic;

namespace MyProyect_Granja.Models
{
    public partial class StockHuevo
    {
        public string Tamano { get; set; } = null!;
        public int Cajas { get; set; }
        public int CartonesExtras { get; set; }
        public int HuevosSueltos { get; set; }
    }
}
