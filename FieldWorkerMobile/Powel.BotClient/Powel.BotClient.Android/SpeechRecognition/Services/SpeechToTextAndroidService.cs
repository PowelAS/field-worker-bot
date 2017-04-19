using System.Collections.Generic;
using Android.Speech.Tts;
using Powel.BotClient.Droid.SpeechRecognition.Services;
using Powel.BotClient.SpeechRecognition.Services.Abstract;
using Xamarin.Forms;


[assembly: Xamarin.Forms.Dependency(typeof(SpeechToTextAndroidService))]
namespace Powel.BotClient.Droid.SpeechRecognition.Services
{
    public class SpeechToTextAndroidService : Java.Lang.Object, ISpeechToTextService, TextToSpeech.IOnInitListener
    {
        TextToSpeech speaker;
        string toSpeak;

        public void Speak(string text)
        {
            var ctx = Forms.Context; // useful for many Android SDK features
            toSpeak = text;
            if (speaker == null)
            {
                speaker = new TextToSpeech(ctx, this);
            }
            else
            {
                var p = new Dictionary<string, string>();
                speaker.Speak(toSpeak, QueueMode.Flush, p);
            }
        }

        #region IOnInitListener implementation
        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                var p = new Dictionary<string, string>();
                speaker.Speak(toSpeak, QueueMode.Flush, p);
            }
        }
        #endregion
    }
}