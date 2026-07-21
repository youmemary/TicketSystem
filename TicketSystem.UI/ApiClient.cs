using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TicketSystem.Models;

public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient()
    {
        _httpClient = new HttpClient { BaseAddress = new System.Uri("https://localhost:7083/api/") };
    }

    public async Task<List<UserRequest>> GetRequestsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<UserRequest>>("requests");
    }

    public async Task CreateRequestAsync(UserRequest request)
    {
        await _httpClient.PostAsJsonAsync("requests", request);
    }

    public async Task UpdateRequestAsync(UserRequest request)
    {
        await _httpClient.PutAsJsonAsync($"requests/{request.Id}", request);
    }

    public async Task DeleteRequestAsync(System.Guid id)
    {
        await _httpClient.DeleteAsync($"requests/{id}");
    }
}
