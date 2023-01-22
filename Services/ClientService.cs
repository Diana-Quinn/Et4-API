//Services va a interactuar con el contexto(Data) y modelos
using BankAPI.Data;
using BankAPI.Data.BankModels;

using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services;//ubicacion de la clase

public class ClientService
{
     private readonly BankDbContext _context;

     public ClientService(BankDbContext context)//ctor
     {
        _context = context;
     }

     public async Task<IEnumerable<Client>> GetAll()//Devuelve lista de clientes
      {
         return await _context.Clients.ToListAsync(); 
      }

    public async Task<Client?> GetById(int id)//devuelve objeto client o un objeto nulo
    {   
        return await _context.Clients.FindAsync(id);
    }


    public async Task<Client> Create(Client newClient)//objeto de tipo cliente, llamado cliente
    {   //crea cliente
        _context.Clients.Add(newClient);
        await _context.SaveChangesAsync();

        return newClient;
    }

    public async Task Update(int id, Client client)
    {
        var existingClient = await GetById(id); //devuelve registro existente
       
        if (existingClient is not null) //si no es nulo, modificamos
        {
           existingClient.Name = client.Name;
           existingClient.PhoneNumber = client.PhoneNumber;
           existingClient.Email = client.Email;

           await _context.SaveChangesAsync();//guardamos
        }
    }

    public async Task Delete(int id)
    {
        var clientToDelete = await GetById(id);
        if (clientToDelete is not null)
        {
            _context.Clients.Remove(clientToDelete);
            await _context.SaveChangesAsync();
        }
    }
}