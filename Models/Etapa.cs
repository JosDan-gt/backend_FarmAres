using System;
using System.Collections.Generic;

namespace MyProyect_Granja.Models
{
    public partial class Etapa
    {
        public Etapa()
        {
            EstadoLotes = new HashSet<EstadoLote>();
        }

        public int IdEtapa { get; set; }
        public string Nombre { get; set; } = null!;

        public virtual ICollection<EstadoLote> EstadoLotes { get; set; }
    }
}
