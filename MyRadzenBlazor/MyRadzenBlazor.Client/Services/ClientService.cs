using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http.Headers;
using MyRadzenBlazor.Client.Requests;

public class ClientService
{
    private readonly HttpClient _httpClient;

    public ClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(List<ClientRequest>, int)> GetClients(int pageIndex, int pageSize)
    {
        var response = await _httpClient.GetAsync($"api/clients?pageIndex={pageIndex}&pageSize={pageSize}");
        if (response.IsSuccessStatusCode)
        {
            var clients = await response.Content.ReadFromJsonAsync<List<ClientRequest>>();
            var totalCount = int.Parse(response.Headers.GetValues("X-Total-Count").FirstOrDefault());
            return (clients, totalCount);
        }

        return (new List<ClientRequest>(), 0);
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
