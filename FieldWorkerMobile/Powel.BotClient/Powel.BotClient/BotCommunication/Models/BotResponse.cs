using System.Collections.Generic;

namespace Powel.BotClient.BotCommunication.Models
{
    public class BotResponse
    {
        public IEnumerable<Message> Messages { get; set; }
        public string Watermark { get; set; }
    }
}
