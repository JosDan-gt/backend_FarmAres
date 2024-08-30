using Microsoft.AspNetCore.Mvc;
using MyProyect_Granja.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;


namespace MyProyect_Granja.Controllers
{
    [Authorize(Roles = "Admin, User")]
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly GranjaAres1Context _context;

        public DashboardController(GranjaAres1Context context)
        {
            _context = context;
        }

        [HttpGet("infolote/{id}")]
        public async Task<ActionResult<VistaDashboard>> GetVistaDash(int id)
        {
            var resultado = await _context.VistaDashboard
                                          .Where(v => v.IdLote == id)
                                          .FirstOrDefaultAsync();

            if (resultado == null)
            {
                return NotFound();
            }

            return Ok(resultado);
        }

        public class ProduccionDto
        {
            public string FechaRegistro { get; set; }
            public int Produccion { get; set; }
            public int Defectuosos { get; set; }
        }

        [HttpGet("produccion/{idLote}/{periodo}")]
        public IActionResult GetProduccion(int idLote, string periodo)
        {
            // Consulta base para obtener todos los datos relevantes
            var baseQuery = _context.ProduccionGallinas
                .Where(p => p.IdLote == idLote && p.Estado == true)
                .OrderBy(p => p.FechaRegistroP)
                .AsEnumerable();

            var produccion = periodo switch
            {
                "diario" => baseQuery
                    .GroupBy(p => p.FechaRegistroP.Value.Date)
                    .Select(g => new ProduccionDto
                    {
                        FechaRegistro = g.Key.ToString("yyyy-MM-dd"),
                        Produccion = g.Sum(p => p.CantTotal ?? 0), // Manejar el nullable int aquí
                        Defectuosos = g.Sum(p => p.Defectuosos ?? 0) // Manejar el nullable int aquí
                    })
                    .ToList(),

                "semanal" => baseQuery
                    .GroupBy(p => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(p.FechaRegistroP.Value, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday))
                    .Select(g => new ProduccionDto
                    {
                        FechaRegistro = $"Semana {g.Key}",
                        Produccion = g.Sum(p => p.CantTotal ?? 0), // Manejar el nullable int aquí
                        Defectuosos = g.Sum(p => p.Defectuosos ?? 0) // Manejar el nullable int aquí
                    })
                    .ToList(),

                "mensual" => baseQuery
                    .GroupBy(p => new { p.FechaRegistroP.Value.Year, p.FechaRegistroP.Value.Month })
                    .Select(g => new ProduccionDto
                    {
                        FechaRegistro = $"{g.Key.Year}-{g.Key.Month:D2}",
                        Produccion = g.Sum(p => p.CantTotal ?? 0), // Manejar el nullable int aquí
                        Defectuosos = g.Sum(p => p.Defectuosos ?? 0) // Manejar el nullable int aquí
                    })
                    .ToList(),

                _ => throw new ArgumentException("Período no válido")
            };

            return Ok(produccion);
        }

        [HttpGet("clasificacion/{idLote}/{periodo}")]
        public IActionResult GetClasificacion(int idLote, string periodo)
        {
            // Consulta base para obtener todos los datos relevantes
            var baseQuery = _context.ClasificacionHuevos
                .Where(c => c.IdProdNavigation.IdLote == idLote && c.Estado == true)
                .OrderBy(c => c.FechaClaS)
                .AsEnumerable();

            var clasificacion = periodo switch
            {
                "diario" => baseQuery
                    .GroupBy(c => c.FechaClaS.Value.Date)
                    .Select(g => new ClasificacionDto
                    {
                        FechaRegistro = g.Key.ToString("yyyy-MM-dd"),
                        TotalUnitaria = g.Sum(c => c.TotalUnitaria)
                    })
                    .ToList(),

                "semanal" => baseQuery
                    .GroupBy(c => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(c.FechaClaS.Value, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday))
                    .Select(g => new ClasificacionDto
                    {
                        FechaRegistro = $"Semana {g.Key}",
                        TotalUnitaria = g.Sum(c => c.TotalUnitaria)
                    })
                    .ToList(),

                "mensual" => baseQuery
                    .GroupBy(c => new { c.FechaClaS.Value.Year, c.FechaClaS.Value.Month })
                    .Select(g => new ClasificacionDto
                    {
                        FechaRegistro = $"{g.Key.Year}-{g.Key.Month:D2}",
                        TotalUnitaria = g.Sum(c => c.TotalUnitaria)
                    })
                    .ToList(),

                _ => throw new ArgumentException("Período no válido")
            };

            return Ok(clasificacion);
        }

        public class ClasificacionDto
        {
            public string FechaRegistro { get; set; }
            public int? TotalUnitaria { get; set; }
        }



        private int GetWeekOfYear(DateTime date)
        {
            var calendar = System.Globalization.CultureInfo.CurrentCulture.Calendar;
            return calendar.GetWeekOfYear(date, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

    }
}
