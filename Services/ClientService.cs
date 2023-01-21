//Services va a interactuar con el contexto(Data) y modelos
using BankAPI.Data;
using BankAPI.Data.BankModels;

namespace BankAPI.Services;//ubicacion de la clase

public class ClientService
{
     private readonly BankDbContext _context;

     public ClientService(BankDbContext context)//ctor
     {
        _context = context;
     }

     public IEnumerable<Client> GetAll()//Devuelve lista de clientes
      {
         return _context.Clients.ToList(); 
      }

    public Client? GetById(int id)//devuelve objeto client o un objeto nulo
    {   
        return _context.Clients.Find(id);
    }


    public Client Create(Client newClient)//objeto de tipo cliente, llamado cliente
    {   //crea cliente
        _context.Clients.Add(newClient);
        _context.SaveChanges();

        return newClient;
    }

    public void Update(int id,Client client)
    {
        var existingClient = GetById(id); //devuelve registro existente
       
        if (existingClient is not null) //si no es nulo, modificamos
        {
           existingClient.Name = client.Name;
           existingClient.PhoneNumber = client.PhoneNumber;
           existingClient.Email = client.Email;

           _context.SaveChanges();//guardamos
        }
    }

    public void Delete(int id)
    {
        var clientToDelete = GetById(id);
        if (clientToDelete is not null)
        {
            _context.Clients.Remove(clientToDelete);
            _context.SaveChanges();
        }
    }
}