using Microsoft.AspNetCore.Mvc;//ControllerBase
//using BankAPI.Data;//Para acceder a BankContext
using BankAPI.Services;
using BankAPI.Data.BankModels;//para acceder a Client
using TestBankAPI.Data.DTOs;

namespace BankAPI.Controllers; //nombre projecto . ubicacion de clase

[ApiController]
[Route("[controller]")]
public class BankTransactionController : ControllerBase
{
    
    private readonly AccountService accountService; // read only
    private readonly AccountTypeService accountTypeService;
    private readonly ClientService clientService;
    private readonly TransactionTypeService transactionTypeService;
    private readonly BankTransactionService bankTransactionService;


    public BankTransactionController(
        AccountService accountService,
        AccountTypeService accountTypeService,
        ClientService clientService,
        TransactionTypeService transactionTypeService,
        BankTransactionService bankTransactionService)
    {
        this.accountService = accountService;
        this.accountTypeService = accountTypeService;
        this.clientService = clientService;
        this.transactionTypeService = transactionTypeService;
        this.bankTransactionService = bankTransactionService;
    }
    
    [HttpGet] //Solicitud GET
    public async Task<IEnumerable<BankTransaction>> Get()//Devuelve lista de bank transactions 
    {
        return await bankTransactionService.GetAll(); 
    }

    [HttpGet("{id}")] 
    public async Task<ActionResult<BankTransaction>> GetById(int id) //Objeto ActionResult obtiene diferentes metodos de la clase ControllerBase
    {
        var bankTransaction = await bankTransactionService.GetById(id); //devuelve registro existente

        if (bankTransaction is null) //no se encontró un registro con ese ID
            return BankTransactionNotFound(id);//404
        
        return bankTransaction; // SI SE ENCUENTRA, DEVUELVE A ESE CLIENTE
    }
    

    [HttpPost]
    public async Task<IActionResult> Create(BankTransactionDTO bankTransaction)//objeto de tipo cliente, llamado cliente
    {   //crea cliente
        var newBankTransaction = await bankTransactionService.Create(bankTransaction);
        await this.UpdateAccountBalance(newBankTransaction);

        //devuelve info cliente
        return CreatedAtAction(nameof(GetById), new { id = newBankTransaction.Id}, newBankTransaction );//201 created
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, BankTransactionDTO bankTransaction)
    {
        if(id != bankTransaction.Id)//si el id que enviamos en la solicitud es diferente al del objeto
            return BadRequest((new { message = $"El ID = {id} de la URL no coincide con el ID = {bankTransaction.Id} del cuerpo de la solicitud"}));//400
        
        var bankTransactionToUpdate = await bankTransactionService.GetById(id); //devuelve registro existente
        
        if (bankTransactionToUpdate is not null) //si existe
        {
           await bankTransactionService.Update(bankTransaction);

             return NoContent();
        }
        else
        {
            return BankTransactionNotFound(id);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var bankTransactionToDelete = await bankTransactionService.GetById(id);

        if(bankTransactionToDelete is not null)
        {
            await bankTransactionService.Delete(id);
            return Ok();
        }
        else
        {
            return BankTransactionNotFound(id);
        }
    }

    public NotFoundObjectResult BankTransactionNotFound(int id)
    {
        return NotFound(new { message = $"El Bank Transaction con ID = {id} no existe."});
    }
    

    public async Task UpdateAccountBalance(BankTransaction bankTransaction)
    {
        // seleccionamos la cuenta de donde se hace la transaccion
        var account = bankTransaction.Account;
        
        // hacer la suma/resta correspondiente
        switch (bankTransaction.TransactionType) {
            case 1: // Depositos
            case 3:
                account.Balance += bankTransaction.Amount;
            break;

            case 2: // Retiros
            case 4:
                account.Balance -= bankTransaction.Amount;
            break;

            default:
            break;
        }

        // actualizamos la cuenta en la bd
        await accountService.Update(new AccountDtoIn(account) );
    }


}