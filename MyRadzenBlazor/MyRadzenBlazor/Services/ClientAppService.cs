using MyRadzenBlazor.Entities;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyRadzenBlazor.Services
{
    public class ClientAppService
    {
        private readonly IMemoryCache _cache;
        private const string CacheKey = "clients";

        public ClientAppService(IMemoryCache cache)
        {
            _cache = cache;
        }

        private List<Cliente> LoadMockData()
        {
            return new List<Cliente>
            {
                new Cliente { Id = 1, Name = "John Doe", Email = "john.doe@example.com", Phone = "123-456-7890" },
                new Cliente { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com", Phone = "234-567-8901" },
                new Cliente { Id = 3, Name = "Samuel Green", Email = "samuel.green@example.com", Phone = "345-678-9012" },
                new Cliente { Id = 4, Name = "Nancy Brown", Email = "nancy.brown@example.com", Phone = "456-789-0123" },
                new Cliente { Id = 5, Name = "Michael White", Email = "michael.white@example.com", Phone = "567-890-1234" },
                new Cliente { Id = 6, Name = "Linda Black", Email = "linda.black@example.com", Phone = "678-901-2345" },
            };
        }

        public List<Cliente> GetClients()
        {
            if (!_cache.TryGetValue(CacheKey, out List<Cliente> clients))
            {
                clients = LoadMockData();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));
                _cache.Set(CacheKey, clients, cacheEntryOptions);
            }

            return clients.OrderByDescending(c => c.Id).ToList();
        }

        public Cliente GetClientById(int id)
        {
            var clients = GetClients();
            return clients.FirstOrDefault(c => c.Id == id);
        }

        public void CreateClient(Cliente client)
        {
            var clients = GetClients();
            client.Id = clients.Max(c => c.Id) + 1;
            clients.Add(client);
            _cache.Set(CacheKey, clients);
        }

        public void UpdateClient(Cliente client)
        {
            var clients = GetClients();
            var existingClient = clients.FirstOrDefault(c => c.Id == client.Id);
            if (existingClient != null)
            {
                existingClient.Name = client.Name;
                existingClient.Email = client.Email;
                existingClient.Phone = client.Phone;
                _cache.Set(CacheKey, clients);
            }
        }

        public void DeleteClient(int id)
        {
            var clients = GetClients();
            var client = clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
            {
                clients.Remove(client);
                _cache.Set(CacheKey, clients);
            }
        }
    }
}
