//Services va a interactuar con el contexto(Data) y modelos
using BankAPI.Data;
using BankAPI.Data.BankModels;

using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services;//ubicacion de la clase

public class AccountTypeService
{
     private readonly BankDbContext _context;

     public AccountTypeService(BankDbContext context)//ctor
     {
        _context = context;
     }

    public async Task<AccountType?> GetById(int id)//devuelve objeto client o un objeto nulo
    {   
        return await _context.AccountTypes.FindAsync(id);
    }

}