using BankAPI.Data;
using BankAPI.Data.BankModels;
using TestBankAPI.Data.DTOs;

using Microsoft.EntityFrameworkCore;
using BankAPI.Data.DTOs;

namespace BankAPI.Services;//ubicacion de la clase

public class LoginService
{
    private readonly BankDbContext _context;

    public LoginService(BankDbContext context)
    {
        _context = context;
        
    }

    public async Task<Administrator?> GetAdmin(AdminDTO admin)//Devuelve lista 
    {
         return await _context.Administrators.
         SingleOrDefaultAsync(x => x.Email == admin.Email && x.Pwd == admin.Pwd);
    }


}