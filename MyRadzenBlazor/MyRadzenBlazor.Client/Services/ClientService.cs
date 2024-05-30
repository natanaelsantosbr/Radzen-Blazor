using MyRadzenBlazor.Client.Requests;
using System.Net.Http.Json;

namespace MyRadzenBlazor.Client.Services
{
    public class ClientService
    {
        private readonly HttpClient _httpClient;

        public ClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ClientRequest>> GetClients()
        {
            return await _httpClient.GetFromJsonAsync<List<ClientRequest>>("api/clients");
        }

        public async Task<ClientRequest> GetClientById(int id)
        {
            return await _httpClient.GetFromJsonAsync<ClientRequest>($"api/clients/{id}");
        }

        public async Task CreateClient(ClientRequest client)
        {
            await _httpClient.PostAsJsonAsync("api/clients", client);
        }

        public async Task UpdateClient(ClientRequest client)
        {
            await _httpClient.PutAsJsonAsync($"api/clients/{client.Id}", client);
        }

        public async Task DeleteClient(int id)
        {
            await _httpClient.DeleteAsync($"api/clients/{id}");
        }
    }
}
