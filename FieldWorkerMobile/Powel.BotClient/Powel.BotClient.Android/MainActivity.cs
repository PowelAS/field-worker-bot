using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Speech;
using Plugin.Permissions;
using Powel.BotClient.Droid.SpeechRecognition.Models;
using Powel.BotClient.Droid.SpeechRecognition.Services;

namespace Powel.BotClient.Droid
{
    [Activity(Label = "Powel Bot Client", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public event EventHandler<SpeechRecognizedEvent> SpeechRecognized;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == SpeechRecognitionAndroidService.SpeakRequestCode)
            {
                if (resultCode == Result.Ok)
                {
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);

                    if (matches.Count != 0)
                    {
                        Console.WriteLine(matches[0]);

                        SpeechRecognized?.Invoke(
                            this, new SpeechRecognizedEvent(recognizedText: matches[0]));
                    }
                    else
                    {
                        ReturnEmptyResponse();
                    }
                }

                else
                {
                    ReturnEmptyResponse();
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void ReturnEmptyResponse()
        {
            SpeechRecognized?.Invoke(this, new SpeechRecognizedEvent(recognizedText: ""));
        }
    }
}

