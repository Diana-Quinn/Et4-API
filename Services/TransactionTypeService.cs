//Services va a interactuar con el contexto(Data) y modelos
using BankAPI.Data;
using BankAPI.Data.BankModels;

using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services;//ubicacion de la clase

public class TransactionTypeService
{
     private readonly BankDbContext _context;

     public TransactionTypeService(BankDbContext context)//ctor
     {
        _context = context;
     }

    public async Task<TransactionType?> GetById(int id)
    {   
        return await _context.TransactionTypes.FindAsync(id);
    }

}