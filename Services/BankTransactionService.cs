//Services va a interactuar con el contexto(Data) y modelos
using BankAPI.Data;
using BankAPI.Data.BankModels;

using Microsoft.EntityFrameworkCore;
using TestBankAPI.Data.DTOs;

namespace BankAPI.Services;//ubicacion de la clase

public class BankTransactionService
{
     private readonly BankDbContext _context;

     public BankTransactionService(BankDbContext context)//ctor
     {
        _context = context;
     }

     public async Task<IEnumerable<BankTransaction>> GetAll()//Devuelve lista de clientes
      {
         return await _context.BankTransactions.ToListAsync(); 
      }

    public async Task<BankTransaction?> GetById(int id)//devuelve objeto Bank Transaction o un objeto nulo
    {   
        return await _context.BankTransactions.FindAsync(id);
    }


    public async Task<BankTransaction> Create(BankTransactionDTO newBankTransactionDTO)
    {   
        var newBankTransaction = new BankTransaction();

        //newBankTransaction.Id = newBankTransaction.Id;
        newBankTransaction.AccountId =  newBankTransactionDTO.AccountId;
        newBankTransaction.TransactionType = newBankTransactionDTO.TransactionType;
        newBankTransaction.Amount = newBankTransactionDTO.Amount;
        newBankTransaction.ExternalAccount = newBankTransactionDTO.ExternalAccount;
        
        //crea cliente
        _context.BankTransactions.Add(newBankTransaction);
        await _context.SaveChangesAsync();

        return newBankTransaction;
    }

    public async Task Update(BankTransactionDTO bankTransaction)
    {
        var existingBankTransaction = await GetById(bankTransaction.Id); //devuelve registro existente
       
        if (existingBankTransaction is not null) //si no es nulo, modificamos info
        {
           existingBankTransaction.AccountId = bankTransaction.AccountId;
           existingBankTransaction.TransactionType = bankTransaction.TransactionType;
           existingBankTransaction.Amount = bankTransaction.Amount;
           existingBankTransaction.ExternalAccount = bankTransaction.ExternalAccount;
           await _context.SaveChangesAsync();//guardamos

        }
    }

    public async Task Delete(int id)
    {
        var bankTransactionToDelete = await GetById(id);
        if (bankTransactionToDelete is not null)
        {
            _context.BankTransactions.Remove(bankTransactionToDelete);
            await _context.SaveChangesAsync();
        }
    }
}