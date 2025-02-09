using System.Text.Json;
using Report.Application.Interfaces.Services;
namespace Report.Infrastructure.Services;

public class PhoneDirectoryService : IPhoneDirectoryService
{
    private readonly HttpClient _httpClient;

    public PhoneDirectoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LocationStatistics> GetLocationStatistics(string location)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/statistics/location/{location}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LocationStatistics>(content);
        }
        catch (Exception ex)
        {
           
            throw new Exception($"Error getting location statistics: {ex.Message}", ex);
        }
    }
}
