using MyProyect_Granja.Models;
using MyProyect_Granja;

public class RegistrarVentaRequestDto
{
    public Venta Venta { get; set; }
    public List<InsertarDetallesVentaCompletaDto> DetallesVenta { get; set; }
}
