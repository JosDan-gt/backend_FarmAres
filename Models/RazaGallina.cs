using System;
using System.Collections.Generic;

namespace MyProyect_Granja.Models
{
    public partial class RazaGallina
    {
        public RazaGallina()
        {
            Lotes = new HashSet<Lote>();
        }

        public int IdRaza { get; set; }
        public string Raza { get; set; } = null!;
        public string Origen { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string ColorH { get; set; } = null!;
        public string? CaractEspec { get; set; }
        public bool? Estado { get; set; }

        public virtual ICollection<Lote> Lotes { get; set; }
    }
}
