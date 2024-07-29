using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using OrderApi.Models;

public class WhatsAppService : IWhatsAppService
{
    private readonly HttpClient _httpClient;

    public WhatsAppService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task SendMessageConfirmation(string phoneNumber, MessageRequest message)
    {
        var messageContent = new
        {
            phone = phoneNumber,
            body = $"Hello {message.Name}, your order with ID {message.Id} is confirmed."
        };

        var response = await _httpClient.PostAsJsonAsync("https://api.whatsapp.com/send", messageContent);
        response.EnsureSuccessStatusCode();
    }
}
