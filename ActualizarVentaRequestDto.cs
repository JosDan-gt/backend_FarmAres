namespace MyProyect_Granja
{
    public class ActualizarVentaRequestDto
    {
        public int VentaId { get; set; }
        public int ClienteId { get; set; }
        public List<ActualizarDetallesVentaDto> DetallesVenta { get; set; }
    }
}
