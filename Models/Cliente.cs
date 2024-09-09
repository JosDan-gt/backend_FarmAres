using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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

        [JsonIgnore] // Evitar ciclos de referencia
        public virtual ICollection<Venta> Venta { get; set; }
    }
}
