﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyProyect_Granja.Models;

namespace MyProyect_Granja.Services
{
    public class RazaGService : IRazaGService
    {
        private readonly GranjaAres1Context _context;

        public RazaGService(GranjaAres1Context context)
        {
            _context = context;
        }

        public async Task AddRazaAsync(RazaGallina razaGallina)
        {
            var parameters = new[]
            {
                new SqlParameter("@nuevaRaza", razaGallina.Raza),
                new SqlParameter("@nuevoOrigen", razaGallina.Origen),
                new SqlParameter("@nuevoColor", razaGallina.Color),
                new SqlParameter("@nuevoColorH", razaGallina.ColorH),
                new SqlParameter("@nuevaCaractEspec", razaGallina.CaractEspec)

            };

            await _context.Database.ExecuteSqlRawAsync("EXEC [dbo].[InsertRazaG] @nuevaRaza, @nuevoOrigen, @nuevoColor, @nuevoColorH, @nuevaCaractEspec", parameters);

        }

        public async Task UpdRazaAsync(RazaGallina razaGallina)
        {
            var parameters = new[]
            {
                new SqlParameter("@Id_Raza", razaGallina.IdRaza),
                new SqlParameter("@nuevaRaza", razaGallina.Raza),
                new SqlParameter("@nuevoOrigen", razaGallina.Origen),
                new SqlParameter("@nuevoColor", razaGallina.Color),
                new SqlParameter("@nuevoColorH", razaGallina.ColorH),
                new SqlParameter("@nuevaCaractEspec", razaGallina.CaractEspec)

            };

            await _context.Database.ExecuteSqlRawAsync("EXEC [dbo].[ActualizartRazaG] @Id_Raza, @nuevaRaza, @nuevoOrigen, @nuevoColor, @nuevoColorH, @nuevaCaractEspec", parameters);

        }
    }
}


//    @nuevaRaza VARCHAR(90),
//    @nuevoOrigen VARCHAR(90),
//    @nuevoColor VARCHAR(90),
//    @nuevoTamano FLOAT,
//    @nuevoColorH VARCHAR(90),
//    @nuevoPesoProm FLOAT,
//    @nuevaCaractEspec VARCHAR(5000)