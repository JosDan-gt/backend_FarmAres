﻿using System;
using System.Collections.Generic;

namespace MyProyect_Granja.Models
{
    public partial class Venta
    {
        public Venta()
        {
            DetallesVenta = new HashSet<DetallesVentum>();
        }

        public int VentaId { get; set; }
        public DateTime FechaVenta { get; set; }
        public int? ClienteId { get; set; }
        public decimal? TotalVenta { get; set; }
        public bool? Estado { get; set; }

        public virtual Cliente? Cliente { get; set; }
        public virtual ICollection<DetallesVentum> DetallesVenta { get; set; }
    }
}
