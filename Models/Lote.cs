using System;
using System.Collections.Generic;

namespace MyProyect_Granja.Models
{
    public partial class Lote
    {
        public Lote()
        {
            EstadoLotes = new HashSet<EstadoLote>();
            ProduccionGallinas = new HashSet<ProduccionGallina>();
        }

        public int IdLote { get; set; }
        public string? NumLote { get; set; }
        public int CantidadG { get; set; }
        public int IdRaza { get; set; }
        public DateTime FechaAdq { get; set; }
        public int IdCorral { get; set; }
        public int? CantidadGctual { get; set; }
        public bool? Estado { get; set; }
        public bool EstadoBaja { get; set; }

        public virtual Corral? IdCorralNavigation { get; set; } = null!;
        public virtual RazaGallina? IdRazaNavigation { get; set; } = null!;
        public virtual ICollection<EstadoLote> EstadoLotes { get; set; }
        public virtual ICollection<ProduccionGallina> ProduccionGallinas { get; set; }
    }
}
