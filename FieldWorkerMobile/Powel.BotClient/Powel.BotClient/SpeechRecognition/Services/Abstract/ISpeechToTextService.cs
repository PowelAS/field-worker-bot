using System.Threading.Tasks;

namespace Powel.BotClient.SpeechRecognition.Services.Abstract
{
    public interface ISpeechRecognitionService
    {
        // todo: cancellation token
        Task<string> Recognize();
    }
}