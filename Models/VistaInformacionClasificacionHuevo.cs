using System;
using System.Collections.Generic;

namespace MyProyect_Granja.Models
{
    public partial class VistaInformacionClasificacionHuevo
    {
        public string Tamaño { get; set; } = null!;
        public int Cajas { get; set; }
        public int CartonesExtras { get; set; }
        public int HuevosSueltos { get; set; }
        public int IdProduccion { get; set; }
        public int? CantidadTotalProduccion { get; set; }
        public int? CantidadTotalClasificada { get; set; }
    }
}
