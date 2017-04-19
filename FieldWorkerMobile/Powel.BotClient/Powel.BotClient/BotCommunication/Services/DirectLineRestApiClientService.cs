using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using Powel.BotClient.BotCommunication.Models;
using Newtonsoft.Json;
using Powel.BotClient.BotCommunication.Services.Abstract;

namespace Powel.BotClient.BotCommunication.Services
{
    public class DirectLineRestApiClientService : IDirectLineClientService
    {
        private readonly HttpClient _client;
        readonly string _directLineKey;
        private ConversationStartedResponse _conversationStartedResponse;

        public DirectLineRestApiClientService(HttpClient client, string directLineKey)
        {
            _client = client;
            _directLineKey = directLineKey;
        }

        public async Task<T> ReceiveAsync<T>()
        {
            InitializeHeaders(_client, _conversationStartedResponse.Token);
            var response = await _client.GetAsync($"https://directline.botframework.com/api/conversations/{_conversationStartedResponse.ConversationId}/messages");
            var result = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<T>(result);

            return json;
        }

        public async Task SendAsync<T>(T item)
        {
            InitializeHeaders(_client, _conversationStartedResponse.Token);
            var content = JsonConvert.SerializeObject(item);

            await _client.PostAsync($"https://directline.botframework.com/api/conversations/{_conversationStartedResponse.ConversationId}/messages", new StringContent(content, Encoding.UTF8, "application/json"));
        }

        public async Task StartConversationAsync()
        {
            InitializeHeaders(_client, _directLineKey);
            var response = await _client.PostAsync("https://directline.botframework.com/api/conversations", new StringContent(""));
            if (response.IsSuccessStatusCode)
            {
                var conversationStartedResponseJson = await response.Content.ReadAsStringAsync();
                _conversationStartedResponse = JsonConvert.DeserializeObject<ConversationStartedResponse>(conversationStartedResponseJson);
            }
        }

        private void InitializeHeaders(HttpClient client, string token)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
