using MyProyect_Granja.Models;
using System.Threading.Tasks;

namespace MyProyect_Granja.Services
{
    public interface IClasificacionHuevoService
    {
        Task AddClasificacionHuevoAsync(ClasificacionHuevo clasificacionHuevo);
        Task UpdateClasificacionHuevosAsync(ClasificacionHuevo UpClasificacionHuevo);
    }

    public interface ICorralService
    {
        Task AddCorralAsync(Corral corral);
        Task UpdCorralAsync(Corral corral);
    }

    public interface IEstadoLoteService
    {
        Task AddEstadoLoteAsync(EstadoLote estadoLote);
        Task UpdEstadoLoteAsync(EstadoLote estadoLote);
    }

    public interface IProduccionService
    {
        Task AddProduccionAsync(ProduccionGallina produccionGallina);
        Task UpdProduccionAsync(ProduccionGallina produccionGallina);

    }

    public interface IRazaGService
    {
        Task AddRazaAsync(RazaGallina razaGallina);
        Task UpdRazaAsync(RazaGallina razaGallina);
    }

    public interface ILoteService
    {
        Task AddLoteAsync(Lote lote);
        Task UpdLoteAsync(Lote lote);
    }



}


//dotnet ef dbcontext scaffold "Server=LAPTOP-ECOQDBI2;Database=GranjaAres1;Integrated Security=True;" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --context GranjaAres1Context --force
