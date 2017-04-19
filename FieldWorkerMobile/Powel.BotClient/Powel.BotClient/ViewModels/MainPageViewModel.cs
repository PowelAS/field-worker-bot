using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Powel.BotClient.BotCommunication.Services;
using Powel.BotClient.SpeechRecognition.Services.Abstract;
using PropertyChanged;
using Xamarin.Forms;
using Powel.BotClient.BotCommunication.Services.Abstract;
using System.Linq;
using System.Net.Http;
using Powel.BotClient.BotCommunication.Helpers;
using Powel.BotClient.Photo.Services;

namespace Powel.BotClient.ViewModels
{
    [ImplementPropertyChanged]
    public class MainPageViewModel
    {
        private readonly Func<Task> _scrollDown;
        private readonly ISpeechToTextService _speechToTextService;
        private readonly ISpeechRecognitionService _speechRecognitionService;
        private readonly IBotCommunicationService _fieldBotCommunicationService;

        public MainPageViewModel(Func<Task> scrollDown)
        {
            _scrollDown = scrollDown;
            _speechToTextService = DependencyService.Get<ISpeechToTextService>();
            _speechRecognitionService = DependencyService.Get<ISpeechRecognitionService>();
            var directLineClient = new DirectLineRestApiClientService(new HttpClient(), "YOUR_DIRECT_LINE_KEY");
            _fieldBotCommunicationService = new FieldBotCommunicationService(directLineClient, "YOUR_USER_NAME");
        }

        public ObservableCollection<ChatBubbleViewModel> Messages { get; set; } =
            new ObservableCollection<ChatBubbleViewModel>();

        public string ChatInputText { get; set; }

        public double IsTypingMessageVisible { get; set; }

        public ICommand SendClicked
        {
            get
            {
                return new Command(async () =>
                {
                    await EnterPressed();
                });
            }
        }

        public async Task StartBotCommunication()
        {
            await _fieldBotCommunicationService.StartConversationAsync();
        }

        public ICommand OnMicrophoneClicked
        {
            get
            {
                return new Command(async () =>
                {
                    if (_speechRecognitionService == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("",
                            "Speech recognition is not yet available on this platform", "Ok");

                        return;
                    }

                    var recognizedText = await _speechRecognitionService.Recognize();

                    if (string.IsNullOrEmpty(recognizedText))
                    {
                        return;
                    }

                    await SendUserMessageAndShowBotResponse(new ChatBubbleViewModel(recognizedText, false));
                });
            }
        }

        public ICommand OnUploadImageClicked
        {
            get
            {
                return new Command(async () =>
                {
                    var photoService = new PhotoService();
                    var photoBytes = await photoService.TakePhotoAsync();

                    Messages.Add(new ChatBubbleViewModel(photoBytes));
                    ChatInputText = "";
                    await ScrollDown(100);
                });
            }
        }

        public async Task EnterPressed()
        {
            if (string.IsNullOrEmpty(ChatInputText))
            {
                return;
            }
            await SendUserMessageAndShowBotResponse(new ChatBubbleViewModel(ChatInputText, false));
        }

        private async Task SendUserMessageAndShowBotResponse(ChatBubbleViewModel message)
        {
            Messages.Add(message);
            ChatInputText = "";
            await ScrollDown();

            await GetAndShowBotReply(message);
        }

        private async Task GetAndShowBotReply(ChatBubbleViewModel message)
        {
            IsTypingMessageVisible = 1;

            await _fieldBotCommunicationService.SendMessageAsync(message.Text);

            var botResponse = await _fieldBotCommunicationService.GetBotResponseAsync();
            var reply = "Sorry, I don't understand that.";

            if (botResponse.Messages != null)
            {
                var messages = botResponse.Messages.ToList();
                var localConversation = Messages
                    .Select(m => m.Text)
                    .ToList();
                if (BotCommunicationHelpers.IsBotResponseMultiline(messages, localConversation))
                {
                    reply = BotCommunicationHelpers.FormatMultilineBotResponse(messages, localConversation);
                }
                else
                {
                    var lastMessage = messages.Last();
                    reply = lastMessage.Text;
                }
            }
            IsTypingMessageVisible = 0;
            await AddBotMessage(reply);
        }

        private async Task ScrollDown(int thresholdInMs = 15)
        {
            if (Device.OS == TargetPlatform.Windows)
            {
                return;
            }

            // workaround for case when bubble appears after the scrolling
            await Task.Delay(thresholdInMs);
            await _scrollDown.Invoke();
        }

        private async Task AddBotMessage(string text)
        {
            var chatBubbleViewModel = new ChatBubbleViewModel(text, true);
            Messages.Add(chatBubbleViewModel);
            await ScrollDown();

            if (_speechRecognitionService != null)
            {
                _speechToTextService.Speak(text);
            }
        }
    }
}