using System;
using System.Collections.Generic;

namespace MyProyect_Granja.Models
{
    public partial class Producto
    {
        public Producto()
        {
            DetallesVenta = new HashSet<DetallesVentum>();
        }

        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = null!;
        public string? Descripcion { get; set; }
        public bool? Estado { get; set; }

        public virtual ICollection<DetallesVentum> DetallesVenta { get; set; }
    }
}
