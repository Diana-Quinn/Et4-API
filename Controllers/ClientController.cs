using Microsoft.AspNetCore.Mvc;//ControllerBase
using BankAPI.Data;//Para acceder a BankContext
using BankAPI.Data.BankModels;//para acceder a Client

namespace BankAPI.Controllers; //nombre projecto . ubicacion de clase

[ApiController]
[Route("[controller]")]
public class ClientController : ControllerBase
{
    private readonly BankContext _context; // read only
    public ClientController(BankContext context)
    {
        _context =  context;
    }
    
    [HttpGet] //Solicitud GET
    public IEnumerable<Client> Get()//Devuelve lista de clientes
    {
        return _context.Clients.ToList(); 
    }

    [HttpGet("{id}")] 
    public ActionResult<Client> GetById(int id) //Objeto ActionResult obtiene diferentes metodos de la clase ControllerBase
    {
        var client = _context.Clients.Find(id); //devuelve registro existente

        if (client is null) //no se encontró un registro con ese ID
            return NotFound();//404
        
        return client; // SI SE ENCUENTRA, DEVUELVE A ESE CLIENTE
    }

}