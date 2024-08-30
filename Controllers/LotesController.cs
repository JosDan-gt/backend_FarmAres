using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProyect_Granja.Models;
using MyProyect_Granja.Services;

namespace MyProyect_Granja.Controllers
{
    [Authorize(Roles = "Admin, User")]
    [ApiController]
    [Route("api/lotes")]
    public class LotesController : Controller
    {
        private readonly GranjaAres1Context _context;
        private readonly ILoteService _loteService;

        public LotesController(GranjaAres1Context context, ILoteService loteService)
        {
            _context = context;
            _loteService = loteService;
        }
        [Authorize(Roles = "Admin, User")]
        [HttpGet("/getlotes")]
        public async Task<ActionResult<IEnumerable<Lote>>> GetLotes()
        {
            var lotes = await _context.Lotes
                                      .Where(l => l.Estado == true && l.EstadoBaja == false) 
                                      .ToListAsync();

            if (lotes == null || !lotes.Any())
            {
                return NotFound("No se encontraron lotes.");
            }

            return Ok(lotes);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/getlotesdadosdebaja")]
        public async Task<ActionResult<IEnumerable<Lote>>> GetLotesDadosDeBaja()
        {
            var lotes = await _context.Lotes
                                      .Where(l => l.EstadoBaja == true)
                                      .ToListAsync();

            if (lotes == null || !lotes.Any())
            {
                return NotFound("No se encontraron lotes dados de baja.");
            }

            return Ok(lotes);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("/postLote")]
        public async Task<IActionResult> Post([FromBody] Lote lote)
        {
            if (lote == null)
            {
                return BadRequest("lote es NULL");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _loteService.AddLoteAsync(lote);
                return Ok(new { success = true, message = "Lote registrado exitosamente." });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("/putLote")]
        public async Task<IActionResult> Put([FromBody] Lote lote)
        {

            if (lote == null)
            {
                return BadRequest("Corral es Null");
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _loteService.UpdLoteAsync(lote);
                return Ok(new { success = true, message = "Lote actualizado correctamente" });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("/updateestadolot")]
        public async Task<IActionResult> PutEstado(int idlote, [FromBody] EstadoDto dto)
        {
            var lote = await _context.Lotes.FindAsync(idlote);
            if (lote == null)
            {
                return NotFound(new { message = "Producción no encontrada." });
            }

            lote.Estado = dto.Estado;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Lote eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("putLoteBaja")]
        public async Task<IActionResult> PutLoteBaja(int idLote, [FromBody] Lote lote)
        {
            var existingLote = await _context.Lotes.FindAsync(idLote);
            if (existingLote == null)
            {
                return NotFound("Lote no encontrado.");
            }

            existingLote.EstadoBaja = lote.EstadoBaja; // Actualiza el campo EstadoBaja
            await _context.SaveChangesAsync();

            return Ok("Estado del lote actualizado correctamente.");
        }



        public class EstadoDto
        {
            public bool Estado { get; set; }
        }

    }

}
