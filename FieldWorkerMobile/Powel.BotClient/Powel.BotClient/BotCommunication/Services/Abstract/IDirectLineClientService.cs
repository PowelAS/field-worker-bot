using System.Threading.Tasks;

namespace Powel.BotClient.BotCommunication.Services.Abstract
{
    public interface IDirectLineClientService
    {
        Task StartConversationAsync();
        Task SendAsync<T>(T item);
        Task<T> ReceiveAsync<T>();
    }
}