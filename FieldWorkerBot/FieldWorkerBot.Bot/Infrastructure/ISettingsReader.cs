namespace FieldWorkerBot.Bot.Infrastructure
{
    public interface ISettingsReader
    {
        string this[string index] { get; }
    }
}