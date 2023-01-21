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
    public IEnumerable<Client> Get()//Devuelve lista de clientes
    {
        return _service.GetAll(); 
    }

    [HttpGet("{id}")] 
    public ActionResult<Client> GetById(int id) //Objeto ActionResult obtiene diferentes metodos de la clase ControllerBase
    {
        var client = _service.GetById(id); //devuelve registro existente

        if (client is null) //no se encontró un registro con ese ID
            return NotFound();//404
        
        return client; // SI SE ENCUENTRA, DEVUELVE A ESE CLIENTE
    }
    

    [HttpPost]
    public IActionResult Create(Client client)//objeto de tipo cliente, llamado cliente
    {   //crea cliente
        var newClient = _service.Create(client);
        //devuelve info cliente
        return CreatedAtAction(nameof(GetById), new { id = newClient.Id}, client );//201 created
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Client client)
    {
        if(id != client.Id)//si el id que enviamos en la solicitud es diferente al del objeto
            return BadRequest();//400
        
        var clientToUpdate = _service.GetById(id); //devuelve registro existente
        
        if (clientToUpdate is not null) //si existe
        {
            _service.Update(id, client);
             return NoContent();
        }
        else
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var clientToDelete = _service.GetById(id);

        if(clientToDelete is not null)
        {
            _service.Delete(id);
            return Ok();

        }
        else
        {
            return NotFound();
        }
    }

}