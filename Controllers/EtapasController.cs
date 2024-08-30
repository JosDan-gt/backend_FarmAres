using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProyect_Granja.Models;
using MyProyect_Granja.Services;

namespace MyProyect_Granja.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/etapas")]
    public class EtapasController : Controller
    {
        private readonly GranjaAres1Context _context;
        public EtapasController(GranjaAres1Context context)
        {
            _context = context;
        }


        [HttpGet("/getetapas")]
        public async Task<ActionResult<IEnumerable<Etapa>>> GetLotes()
        {
            var etapa = await _context.Etapas.ToListAsync();

            if (etapa == null || !etapa.Any())
            {
                return NotFound("No se encontraron etapas.");
            }

            return Ok(etapa);
        }
    }
}
