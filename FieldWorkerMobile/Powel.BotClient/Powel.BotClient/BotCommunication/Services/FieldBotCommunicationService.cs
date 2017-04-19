using Powel.BotClient.BotCommunication.Services.Abstract;
using Powel.BotClient.BotCommunication.Models;
using System.Threading.Tasks;

namespace Powel.BotClient.BotCommunication.Services
{
    class FieldBotCommunicationService : IBotCommunicationService
    {
        private readonly string _userName;
        readonly IDirectLineClientService _directClientService;

        public FieldBotCommunicationService(IDirectLineClientService directClientService, string userName)
        {
            _userName = userName;
            _directClientService = directClientService;
        }

        public async Task SendMessageAsync(string message)
        {
            await _directClientService.SendAsync(new Message
            {
                Text = message,
                From = _userName                
            });            
        }

        public async Task<BotResponse> GetBotResponseAsync()
        {
            var botResponse = await _directClientService.ReceiveAsync<BotResponse>();

            return botResponse;
        }
      
        public async Task StartConversationAsync()
        {
            await _directClientService.StartConversationAsync();
        }
    }
}
