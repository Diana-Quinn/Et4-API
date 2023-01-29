using Microsoft.AspNetCore.Mvc;//ControllerBase
//using BankAPI.Data;//Para acceder a BankContext
using BankAPI.Services;
using BankAPI.Data.BankModels;//para acceder a Client
using TestBankAPI.Data.DTOs;

namespace BankAPI.Controllers; //nombre projecto . ubicacion de clase

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly AccountService accountService; // read only
    private readonly AccountTypeService accountTypeService;
    private readonly ClientService clientService;

    public AccountController(AccountService accountService,
                            AccountTypeService accountTypeService,
                            ClientService clientService)
    {
        this.accountService = accountService;
        this.accountTypeService = accountTypeService;
        this.clientService = clientService;
    }
    
    [HttpGet] //Solicitud GET
    public async Task<IEnumerable<AccountDtoOut>> Get()
    {
        return await accountService.GetAll(); 
    }

    [HttpGet("{id}")] 
    public async Task<ActionResult<AccountDtoOut>> GetById(int id) 
    {
        var account = await accountService.GetDtoById(id); 

        if (account is null) 
            return AccountNotFound(id);//404
        
        return account; 
    }
    

    [HttpPost]
    public async Task<IActionResult> Create(AccountDtoIn account)//objeto de tipo cliente, llamado cliente
    {
        string validationResult = await ValidateAccount(account);

        if(!validationResult.Equals("Valid"))
            return BadRequest(new { message = validationResult });
        
        var newAccount = await accountService.Create(account);

        return CreatedAtAction(nameof(GetById), new { id = newAccount.Id}, newAccount);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, AccountDtoIn account)
    {
        if(id != account.Id)//si el id que enviamos en la solicitud es diferente al del objeto
            return BadRequest(new { message = $"El ID = {id} de la URL no coincide con el ID = {account.Id} del cuerpo de la solicitud"});//400
        
        var accountToUpdate = await accountService.GetById(id); //devuelve registro existente
        
        if (accountToUpdate is not null) //si existe
        {
            
           string validationResult = await ValidateAccount(account);

           if( !validationResult.Equals("Valid") )
                 return BadRequest(new { message = validationResult });

            await accountService.Update(account);
            return NoContent();
        }
        else
        {
            return AccountNotFound(id);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var accountToDelete = await accountService.GetById(id);

        if (accountToDelete is null)
            return AccountNotFound(id);
        
        if (accountToDelete.Balance != 0)
            return BadRequest("El balance debe ser 0");

        await accountService.Delete(id);
        return Ok();

    }

    public NotFoundObjectResult AccountNotFound(int id)
    {
        return NotFound(new { message = $"La cuenta con ID = {id} no existe."});
    }

    public async Task<string> ValidateAccount(AccountDtoIn account)
    {
        string result = "Valid";

        var accountType = await accountTypeService.GetById(account.AccountType);

        if (accountType is null)
            result = $"El tipo de cuenta {account.AccountType} no existe";

        var clientID = account.ClientId.GetValueOrDefault();

        var client = await clientService.GetById(clientID);

        if(client is null)
            result = $"El cliente {clientID} no existe";
            
        return result;
    }


}