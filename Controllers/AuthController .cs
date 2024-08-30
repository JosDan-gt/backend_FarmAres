using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MyProyect_Granja.Models;
using Microsoft.EntityFrameworkCore;

namespace MyProyect_Granja.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly GranjaAres1Context _context;

        public AuthController(IConfiguration configuration, GranjaAres1Context context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var user = await _context.Usuarios
                             .Include(u => u.Role)
                             .SingleOrDefaultAsync(u => u.NombreUser == login.Username);

            if (user == null || login.Password != user.Contrasena)
            {
                return Unauthorized();
            }

            var token = GenerateJwtToken(user);

            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Usuario usuario)
        {
            if (await _context.Usuarios.AnyAsync(u => u.NombreUser == usuario.NombreUser || u.Email == usuario.Email))
            {
                return BadRequest("El nombre de usuario o el correo electrónico ya están en uso.");
            }

            usuario.FechaDeRegistro = DateTime.Now;
            usuario.Estado = true;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuario registrado exitosamente" });
        }

        private string GenerateJwtToken(Usuario user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.NombreUser),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (user.Role != null && !string.IsNullOrEmpty(user.Role.Nombre))
            {
                claims.Add(new Claim(ClaimTypes.Role, user.Role.Nombre));
            }
            else
            {
                throw new InvalidOperationException("El usuario no tiene un rol asignado.");
            }

            var expiresInMinutes = double.Parse(_configuration["Jwt:ExpiresInMinutes"]);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(expiresInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
