using System;
using System.Collections.Generic;

namespace MyProyect_Granja.Models
{
    public partial class VwEstadoLotePorSemana
    {
        public int? Semana { get; set; }
        public int IdLote { get; set; }
        public int? TotalGallinas { get; set; }
        public int? TotalBajas { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int IdEtapa { get; set; }
    }
}
