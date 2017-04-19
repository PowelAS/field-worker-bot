using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Speech;
using Powel.BotClient.Droid.SpeechRecognition.Models;
using Powel.BotClient.Droid.SpeechRecognition.Services;
using Powel.BotClient.SpeechRecognition.Services.Abstract;
using Xamarin.Forms;

[assembly: Dependency(typeof(SpeechRecognitionAndroidService))]
namespace Powel.BotClient.Droid.SpeechRecognition.Services
{
    public class SpeechRecognitionAndroidService : Java.Lang.Object, ISpeechRecognitionService
    {
        public const int SpeakRequestCode = 23;
        private readonly MainActivity _context;
        private TaskCompletionSource<string> _taskCompletionSource;

        public SpeechRecognitionAndroidService()
        {
            _context = (MainActivity)Forms.Context;
        }

        public Task<string> Recognize()
        {
            GC.Collect();

            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelWebSearch);
            voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Speak");

            _context.StartActivityForResult(voiceIntent, SpeakRequestCode);
            _taskCompletionSource = new TaskCompletionSource<string>();

            _context.SpeechRecognized += OnSpeechRecognized;

            return _taskCompletionSource.Task;
        }

        private void OnSpeechRecognized(object o, SpeechRecognizedEvent args)
        {
            _taskCompletionSource.SetResult(args.RecognizedText);
            _context.SpeechRecognized -= OnSpeechRecognized;
        }
    }
}