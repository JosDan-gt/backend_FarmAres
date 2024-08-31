using System;
using System.Collections.Generic;

namespace MyProyect_Granja.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            Venta = new HashSet<Venta>();
        }

        public int ClienteId { get; set; }
        public string NombreCliente { get; set; } = null!;
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public bool? Estado { get; set; }

        public virtual ICollection<Venta> Venta { get; set; }
    }
}
