using Microsoft.AspNetCore.Mvc;//ControllerBase
//using BankAPI.Data;//Para acceder a BankContext
using BankAPI.Services;
using BankAPI.Data.BankModels;//para acceder a Client

namespace BankAPI.Controllers; //nombre projecto . ubicacion de clase

[ApiController]
[Route("[controller]")]
public class ClientController : ControllerBase
{
    private readonly ClientService _service; // read only
    public ClientController(ClientService service)
    {
        _service =  service;
    }
    
    [HttpGet] //Solicitud GET
    public async Task<IEnumerable<Client>> Get() //Devuelve lista de clientes
    {
        return await _service.GetAll(); 
    }

    [HttpGet("{id}")] 
    public async Task<ActionResult<Client>> GetById(int id) //Objeto ActionResult obtiene diferentes metodos de la clase ControllerBase
    {
        var client = await _service.GetById(id); //devuelve registro existente

        if (client is null) //no se encontró un registro con ese ID
            return ClientNotFound(id);//404
        
        return client; // SI SE ENCUENTRA, DEVUELVE A ESE CLIENTE
    }
    

    [HttpPost]
    public async Task<IActionResult> Create(Client client)//objeto de tipo cliente, llamado cliente
    {   //crea cliente
        var newClient = await _service.Create(client);
        //devuelve info cliente
        return CreatedAtAction(nameof(GetById), new { id = newClient.Id}, client );//201 created
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Client client)
    {
        if(id != client.Id)//si el id que enviamos en la solicitud es diferente al del objeto
            return BadRequest((new { message = $"El ID = {id} de la URL no coincide con el ID = {client.Id} del cuerpo de la solicitud"}));//400
        
        var clientToUpdate = await _service.GetById(id); //devuelve registro existente
        
        if (clientToUpdate is not null) //si existe
        {
           await _service.Update(id, client);
             return NoContent();
        }
        else
        {
            return ClientNotFound(id);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var clientToDelete = await _service.GetById(id);

        if(clientToDelete is not null)
        {
            await _service.Delete(id);
            return Ok();

        }
        else
        {
            return ClientNotFound(id);
        }
    }

    public NotFoundObjectResult ClientNotFound(int id)
    {
        return NotFound(new { message = $"El cliente con ID = {id} no existe."});
    }

}