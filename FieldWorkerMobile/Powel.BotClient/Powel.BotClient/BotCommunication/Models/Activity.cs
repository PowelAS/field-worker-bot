namespace Powel.BotClient.BotCommunication.Models
{
    public class Activity
    {
        public From From { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public string ChannelId { get; set; }
        public string Id { get; set; }
    }

    public class From
    {
        public string Id { get; set; }
    }
}
