//Services va a interactuar con el contexto(Data) y modelos
using BankAPI.Data;
using BankAPI.Data.BankModels;

using Microsoft.EntityFrameworkCore;
using TestBankAPI.Data.DTOs;

namespace BankAPI.Services;//ubicacion de la clase

public class AccountService
{
     private readonly BankDbContext _context;

     public AccountService(BankDbContext context)//ctor
     {
        _context = context;
     }

     public async Task<IEnumerable<AccountDtoOut>> GetAll()//Devuelve lista 
      {
         return await _context.Accounts.Select( a => new AccountDtoOut
         {
            Id = a.Id,
            AccountName = a.AccountTypeNavigation.Name,//relacion de account con accountType es accountNAVIGATION
            ClientName = a.Client != null ? a.Client.Name : "",
            Balance = a.Balance,
            RegDate = a.RegDate
         }).ToListAsync();
      }

     public async Task<AccountDtoOut?> GetDtoById(int id)//Devuelve lista 
      {
         return await _context.Accounts.
            Where(a => a.Id == id).
            Select( a => new AccountDtoOut
            {
                Id = a.Id,
                AccountName = a.AccountTypeNavigation.Name,//relacion de account con accountType es accountNAVIGATION
                ClientName = a.Client != null ? a.Client.Name : "",
                Balance = a.Balance,
                RegDate = a.RegDate
            }).SingleOrDefaultAsync();//devuelve un objeto AccountDtoOut o un nulo
      }


    public async Task<Account?> GetById(int id)
    {   
        return await _context.Accounts.FindAsync(id);
    }


    public async Task<Account> Create(AccountDtoIn newAccountDTO)
    {   
        var newAccount = new Account();

        newAccount.AccountType = newAccountDTO.AccountType;
        newAccount.ClientId = newAccountDTO.ClientId;
        newAccount.Balance = newAccountDTO.Balance;
        //crea cliente
        _context.Accounts.Add(newAccount);
        await _context.SaveChangesAsync();

        return newAccount;
    }

    public async Task Update(AccountDtoIn account)
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