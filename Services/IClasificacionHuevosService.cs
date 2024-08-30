using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyProyect_Granja.Models;
using System.Threading.Tasks;

namespace MyProyect_Granja.Services
{
    public class ClasificacionHuevoService : IClasificacionHuevoService
    {
        private readonly GranjaAres1Context _context;

        public ClasificacionHuevoService(GranjaAres1Context context)
        {
            _context = context;
        }

        public async Task AddClasificacionHuevoAsync(ClasificacionHuevo clasificacionHuevo)
        {
            var parameters = new[]
            {
                new SqlParameter("@Tamano", clasificacionHuevo.Tamano),
                new SqlParameter("@Cajas", clasificacionHuevo.Cajas),
                new SqlParameter("@CartonesExtras", clasificacionHuevo.CartonesExtras),
                new SqlParameter("@HuevosSueltos", clasificacionHuevo.HuevosSueltos),
                new SqlParameter("@IdProd", clasificacionHuevo.IdProd),
                new SqlParameter("@FechaClas", clasificacionHuevo.FechaClaS)
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC [dbo].[InsertarClasificacionHuevos] @Tamano, @Cajas, @CartonesExtras, @HuevosSueltos, @IdProd, @FechaClas", parameters);
        }


        public async Task UpdateClasificacionHuevosAsync(ClasificacionHuevo UpClasificacionHuevo)
        {
            var parameters = new[]
            {
                new SqlParameter("@IdClasificacion", UpClasificacionHuevo.Id),
                new SqlParameter("@Tamano", UpClasificacionHuevo.Tamano),
                new SqlParameter("@Cajas", UpClasificacionHuevo.Cajas),
                new SqlParameter("@CartonesExtras", UpClasificacionHuevo.CartonesExtras),
                new SqlParameter("@HuevosSueltos", UpClasificacionHuevo.HuevosSueltos),
                new SqlParameter("@IdProd", UpClasificacionHuevo.IdProd)
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC [dbo].[ActualizarClasificacionHuevos] @IdClasificacion, @Tamano, @Cajas, @CartonesExtras, @HuevosSueltos, @IdProd", parameters);
        }
    }
}
