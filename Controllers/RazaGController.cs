﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProyect_Granja.Models;
using MyProyect_Granja.Services;

namespace MyProyect_Granja.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/razaG")]
    public class RazaGController : ControllerBase
    {
        private readonly GranjaAres1Context _context;
        private readonly IRazaGService _razaGService;

        public RazaGController(GranjaAres1Context context, IRazaGService razaGService)
        {
            _context = context;
            _razaGService = razaGService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/getrazaG")]
        public async Task<ActionResult<IEnumerable<RazaGallina>>> GetRaza()
        {
            var datosR = await _context.RazaGallinas
                                       .Where(r => r.Estado == true)
                                       .ToListAsync();
            return Ok(datosR);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/postraza")]
        public async Task<IActionResult> Post([FromBody] RazaGallina razaGallina)
        {
            if (razaGallina == null)
            {
                return BadRequest("RazaG es Null");
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _razaGService.AddRazaAsync(razaGallina);
                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("/putraza")]
        public async Task<IActionResult> Put([FromBody] RazaGallina razaGallina)
        {
            if (razaGallina == null)
            {
                return BadRequest("RazaG es Null");
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _razaGService.UpdRazaAsync(razaGallina);
                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
