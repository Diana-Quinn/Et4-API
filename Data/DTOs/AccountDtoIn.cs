using BankAPI.Data.BankModels;

namespace TestBankAPI.Data.DTOs;

public class AccountDtoIn
{
    public int? Id { get; set; } // puede ser null porque aun no tienes la id, apenas estas creando el objeto

    public int AccountType { get; set; }

    public int? ClientId { get; set; }

    public decimal Balance { get; set; }

    public AccountDtoIn (Account acc) {
        this.Id = acc.Id;
        this.AccountType = acc.AccountType;
        this.ClientId = acc.ClientId;
        this.Balance = acc.Balance;
    }
}