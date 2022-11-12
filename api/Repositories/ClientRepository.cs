﻿using api.Data;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public interface IClientRepository
    {
        Task<Client[]> Get();
        Task<Client[]> SearchClient(string name);
        Task Create(Client client);
        Task Update(Client client);
    }

    public class ClientRepository : IClientRepository
    {
        private readonly DataContext dataContext;

        public ClientRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task Create(Client client)
        {
            await dataContext.AddAsync(client);
            await dataContext.SaveChangesAsync();
        }

        public Task<Client[]> Get()
        {
            return dataContext.Clients.ToArrayAsync();
        }

        public Task<Client[]> SearchClient(string name)
        {
            var clients = dataContext.Clients.Where(x => x.FirstName.Equals(name, StringComparison.OrdinalIgnoreCase) || 
                x.LastName.Equals(name, StringComparison.OrdinalIgnoreCase))
                .Select(c => new Client
                {
                    FirstName = c.FirstName,    
                    LastName = c.LastName,
                    Email = c.Email,
                    Id = c.Id,
                    PhoneNumber = c.PhoneNumber
                }).ToArrayAsync();  

            return clients;
        }

        public async Task Update(Client client)
        {
            var existingClient = await dataContext.Clients.FirstOrDefaultAsync(x => x.Id == client.Id);

            if (existingClient == null)
                return;

            existingClient.FirstName = client.FirstName;
            existingClient.LastName = client.LastName;
            existingClient.Email = client.Email;
            existingClient.PhoneNumber = client.PhoneNumber;

            await dataContext.SaveChangesAsync();
        }
    }
}

