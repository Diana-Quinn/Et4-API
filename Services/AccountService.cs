//Services va a interactuar con el contexto(Data) y modelos
using BankAPI.Data;
using BankAPI.Data.BankModels;

using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services;//ubicacion de la clase

public class AccountService
{
     private readonly BankDbContext _context;

     public AccountService(BankDbContext context)//ctor
     {
        _context = context;
     }

     public async Task<IEnumerable<Account>> GetAll()//Devuelve lista 
      {
         return await _context.Accounts.ToListAsync(); 
      }

    public async Task<Account?> GetById(int id)
    {   
        return await _context.Accounts.FindAsync(id);
    }


    public async Task<Account> Create(Account newAccount)
    {   //crea cliente
        _context.Accounts.Add(newAccount);
        await _context.SaveChangesAsync();

        return newAccount;
    }

    public async Task Update(Account account)
    {
        var existingAccount = await GetById(account.Id); 
       
        if (existingAccount is not null) 
        {
           existingAccount.AccountType = account.AccountType;
           existingAccount.ClientId = account.ClientId;
           existingAccount.Balance = account.Balance;

           await _context.SaveChangesAsync();//guardamos
        }
    }

    public async Task Delete(int id)
    {
        var accountToDelete = await GetById(id);
        if (accountToDelete is not null)
        {
            _context.Accounts.Remove(accountToDelete);
            await _context.SaveChangesAsync();
        }
    }

 
}