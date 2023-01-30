using Microsoft.AspNetCore.Mvc;//ControllerBase
//using BankAPI.Data;//Para acceder a BankContext
using BankAPI.Services;
using BankAPI.Data.BankModels;//para acceder a Client
using TestBankAPI.Data.DTOs;
using BankAPI.Data.DTOs;

using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BankAPI.Controllers; //nombre projecto . ubicacion de clase

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly LoginService loginService; // read only
    private IConfiguration config;

    public LoginController(LoginService loginService, IConfiguration config)
    {
        this.loginService = loginService;
        this.config = config;
    }

    [HttpPost]
    public async Task<IActionResult> Login(AdminDTO adminDTO)
    {
        var admin = await loginService.GetAdmin(adminDTO);

        if (admin is null)
            return BadRequest(new { message = "Credenciales inválidas" } );

        string jwtToken = GenerateToken(admin);

        //generar un token
        return Ok( new { token = jwtToken } );

    }

    //metodo para generar token
    private string GenerateToken(Administrator admin)
    {
        var claims = new[] //arreglo de claims
        {
            new Claim(ClaimTypes.Name, admin.Name),
            new Claim(ClaimTypes.Email, admin.Email)
        };  

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:Key").Value));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var securityToken = new JwtSecurityToken(
                            claims: claims,
                            expires: DateTime.Now.AddMinutes(60),
                            signingCredentials: creds);
        
        string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return token; //cadena serializada
    }
}