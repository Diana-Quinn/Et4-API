namespace TestBankAPI.Data.DTOs;

public class BankTransactionDTO
{
        public int Id { get; set; }

        public int AccountId { get; set; }

        public int TransactionType { get; set; }

        public decimal Amount { get; set; }

        public int? ExternalAccount { get; set; }

}