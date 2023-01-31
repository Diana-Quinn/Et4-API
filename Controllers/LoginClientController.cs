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
public class LoginClientController : ControllerBase
{
    private readonly LoginClientService loginClientService; // read only
    private IConfiguration config;

    public LoginClientController(LoginClientService loginClientService, IConfiguration config)
    {
        this.loginClientService = loginClientService;
        this.config = config;
    }

    [HttpPost]
    public async Task<IActionResult> LoginClient(ClientDTO clientDTO)
    {
        var client = await loginClientService.GetClientLogin(clientDTO);

        if (client is null)
            return BadRequest(new { message = "Credenciales inválidas" } );

        string jwtToken = GenerateToken(client);

        //generar un token
        return Ok( new { token = jwtToken } );

    }

    //metodo para generar token
    private string GenerateToken(Client client)
    {
        var claims = new[] //arreglo de claims
        {
            new Claim(ClaimTypes.Name, client.Name),
            new Claim(ClaimTypes.Email, client.Email)
        };  

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:Key").Value));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var securityToken = new JwtSecurityToken(
                            claims: claims,
                            expires: DateTime.Now.AddMinutes(60),
                            signingCredentials: creds);
        
        string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
//var identity = HttpContext.User.Identity as ClaimsIdentity;
        return token; //cadena serializada
        
    }



}