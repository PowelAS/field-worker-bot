using System;

namespace Powel.BotClient.Droid.SpeechRecognition.Models
{
    public class SpeechRecognizedEvent : EventArgs
    {
        public SpeechRecognizedEvent(string recognizedText)
        {
            RecognizedText = recognizedText;
        }

        public string RecognizedText { get; }
    }
}