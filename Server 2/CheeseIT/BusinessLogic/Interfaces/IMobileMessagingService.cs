using System.Threading.Tasks;

namespace CheeseIT.BusinessLogic.Interfaces
{
    public interface IMobileMessagingService
    {
        Task SendNotification(string token, string title, string body);
    }
}