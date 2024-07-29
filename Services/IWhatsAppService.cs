using System.Threading.Tasks;
using OrderApi.Models;

public interface IWhatsAppService
{
    Task SendMessageConfirmation(string phoneNumber, MessageRequest message);
}
