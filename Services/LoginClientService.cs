using BankAPI.Data;
using BankAPI.Data.BankModels;
using TestBankAPI.Data.DTOs;

using Microsoft.EntityFrameworkCore;
using BankAPI.Data.DTOs;
using System.Security.Claims;

namespace BankAPI.Services;//ubicacion de la clase

public class LoginClientService
{
    private readonly BankDbContext _context;

    public LoginClientService(BankDbContext context)
    {
        _context = context;
        
    }

    public async Task<Client?> GetClientLogin(ClientDTO client)//Devuelve lista 
    {
         return await _context.Clients.
         SingleOrDefaultAsync(x => x.Email == client.Email && 
                                   x.Pwd == client.Pwd);
    }

/*
   public Task<Client?> validarToken(Claim claims)
    {
        var ClaimsIdentity = Claim.Equals(x => x.Type == "Name" && x.Type == "Email").Value;

    }*/
    
}