using Newtonsoft.Json;

namespace Powel.BotClient.BotCommunication.Models
{
    public class ConversationStartedResponse
    {
        public string ConversationId { get; set; }
        public string Token { get; set; }
        public string StreamUrl { get; set; }
        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }
    }
}
