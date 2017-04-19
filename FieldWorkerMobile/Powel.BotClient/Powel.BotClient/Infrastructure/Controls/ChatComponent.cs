using Powel.BotClient.ViewModels;
using Xamarin.Forms;

namespace Powel.BotClient.Infrastructure.Controls
{
    public class ChatComponent : BindableStackLayout<ChatBubble, ChatBubbleViewModel>
    {
        public ChatComponent()
        {
            VerticalOptions = LayoutOptions.End;
            AppentToEnd = true;
            Spacing = 0;
        }
    }
}