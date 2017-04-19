using Powel.BotClient.BotCommunication.Models;
using System.Threading.Tasks;

namespace Powel.BotClient.BotCommunication.Services.Abstract
{
    public interface IBotCommunicationService
    {
        Task StartConversationAsync();
        Task SendMessageAsync(string message);
        Task<BotResponse> GetBotResponseAsync();
    }
}
