using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class WhatsAppService
{
    private readonly HttpClient _httpClient;
    private readonly string _instanceId;
    private readonly string _apiToken;

    public WhatsAppService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _instanceId = configuration["WaApi:InstanceId"];
        _apiToken = configuration["WaApi:ApiToken"];
    }

    public async Task SendMessageAsync(string phoneNumber, string message)
    {
        var requestUrl = $"https://waapi.app/api/v1/instances/{_instanceId}/client/action/send-message";
        var payload = new
        {
            chatId = $"{phoneNumber}@c.us",
            message
        };

        var requestContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiToken);

        var response = await _httpClient.PostAsync(requestUrl, requestContent);
        response.EnsureSuccessStatusCode();
    }
}
