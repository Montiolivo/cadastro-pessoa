using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cadastro_Pessoa.Data;
using Cadastro_Pessoa.Models;

[ApiController]
[Route("api/[controller]")]
[ApiVersionNeutral]
public class AuthController : ControllerBase
{
    private readonly DataContext _context;
    private readonly string _jwtKey = "minha_chave_super_secreta_123!_1234567897878978998789!";

    public AuthController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] Usuario login)
    {
        var user = _context.Usuarios.SingleOrDefault(u => u.Username == login.Username && u.Password == login.Password);
        if (user == null)
            return Unauthorized("Usuário ou senha inválidos");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("minha_chave_super_secreta_123!_1234567897878978998789!");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddHours(12),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return Ok(new { Token = jwtToken });
    }
}
