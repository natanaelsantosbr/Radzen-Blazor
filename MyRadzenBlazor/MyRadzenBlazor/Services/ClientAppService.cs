using MyRadzenBlazor.Entities;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using MyRadzenBlazor.Shared.Models;

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
            var clients = new List<Cliente>();

            for (int i = 1; i <= 100; i++)
            {
                clients.Add(new Cliente
                {
                    Id = i,
                    Name = $"Client {i}",
                    Email = $"client{i}@example.com",
                    Phone = $"123-456-{i:D4}"
                });
            }

            return clients;
        }

        public PagedResponse<Cliente> GetClientsV2(int pageIndex, int pageSize, string name = null, string email = null)
        {
            if (!_cache.TryGetValue(CacheKey, out List<Cliente> clients))
            {
                clients = LoadMockData();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));
                _cache.Set(CacheKey, clients, cacheEntryOptions);
            }

            var filteredClients = clients.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                filteredClients = filteredClients.Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(email))
            {
                filteredClients = filteredClients.Where(c => c.Email.Contains(email, StringComparison.OrdinalIgnoreCase));
            }

            var pagedClients = filteredClients.OrderByDescending(c => c.Id)
                                              .Skip(pageIndex * pageSize)
                                              .Take(pageSize)
                                              .ToList();
            var totalClients = filteredClients.Count();

            return new PagedResponse<Cliente>(pagedClients, pageIndex, pageSize, totalClients);
        }

        private List<Cliente> GetClients(int pageIndex, int pageSize)
        {
            if (!_cache.TryGetValue(CacheKey, out List<Cliente> clients))
            {
                clients = LoadMockData();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));
                _cache.Set(CacheKey, clients, cacheEntryOptions);
            }

            return clients.OrderByDescending(c => c.Id)
                          .Skip(pageIndex * pageSize)
                          .Take(pageSize)
                          .ToList();
        }

        public Cliente GetClientById(int id)
        {
            var clients = GetClients(0, int.MaxValue); // Get all clients
            return clients.FirstOrDefault(c => c.Id == id);
        }

        public void CreateClient(Cliente client)
        {
            var clients = GetClients(0, int.MaxValue); // Get all clients
            client.Id = clients.Max(c => c.Id) + 1;
            clients.Add(client);
            _cache.Set(CacheKey, clients);
        }

        public void UpdateClient(Cliente client)
        {
            var clients = GetClients(0, int.MaxValue); // Get all clients
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
            var clients = GetClients(0, int.MaxValue); // Get all clients
            var client = clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
            {
                clients.Remove(client);
                _cache.Set(CacheKey, clients);
            }
        }
    }
}
