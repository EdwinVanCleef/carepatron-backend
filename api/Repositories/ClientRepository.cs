﻿using api.Data;
using api.Events;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public interface IClientRepository
    {
        Task<Client[]> Get();
        Task<Client[]> SearchClient(string name);
        Task Create(Client client);
        Task Update(string id, Client client);
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
            if (await dataContext.Clients.Where((x) => x.Id == client.Id).AnyAsync())
            {
                throw new ArgumentException($"Client {client.Id} already exists.");
            }

            await dataContext.AddAsync(client);
            await dataContext.SaveChangesAsync();

            // emit event 
            var newClientEventData = new ClientEvent
            {
                Id = new Guid(),
                ClientId = client.Id,
                ClientFirstName = client.FirstName,
                ClientLastName = client.LastName,
                DateCreated = DateTime.Now
            };

            var newClientEventPub = new ClientEventPublisher();
            var newClientEvent = new ClientEventSubscriber(newClientEventData, newClientEventPub);

            newClientEventPub.NewClientEvent(newClientEventData);
            
        }

        public Task<Client[]> Get()
        {
            return dataContext.Clients.ToArrayAsync();
        }

        public Task<Client[]> SearchClient(string name)
        {
            var clients = dataContext.Clients.Where(x => x.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) || 
                x.LastName.Contains(name, StringComparison.OrdinalIgnoreCase))
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

        public async Task Update(string id, Client client)
        {
            var existingClient = await dataContext.Clients.FirstOrDefaultAsync(x => x.Id == id);

            if (existingClient == null)
                throw new ArgumentException($"Client {id} not found.");

            existingClient.FirstName = client.FirstName;
            existingClient.LastName = client.LastName;
            existingClient.Email = client.Email;
            existingClient.PhoneNumber = client.PhoneNumber;

            await dataContext.SaveChangesAsync();
        }

    }
}

