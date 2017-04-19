using System;
using System.IO;
using Powel.BotClient.Infrastructure.Extensions;
using Powel.BotClient.Infrastructure.Helpers;
using PropertyChanged;
using Xamarin.Forms;

namespace Powel.BotClient.ViewModels
{
    [ImplementPropertyChanged]
    public class ChatBubbleViewModel : IEquatable<ChatBubbleViewModel>
    {
        private readonly Guid _id = Guid.NewGuid();

        public ChatBubbleViewModel(byte[] imageBytes) : this("", false)
        {
            Image = ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }

        public ChatBubbleViewModel(string text, bool isBot)
        {
            var powelBlue = ColorHelper.GetColor("PowelBlue");
            var gray = ColorHelper.GetColor("BotBubbleBackground");
            var darkGray = ColorHelper.GetColor("TextDark");

            Text = text.CapitalizeFirstLetter();
            const int sideMargin = 80;
            const int smallMargin = 5;
            const int smallSideMargin = 10;
            Margin = isBot ?
                new Thickness(smallSideMargin, smallMargin, sideMargin, smallMargin)
                : new Thickness(sideMargin, smallMargin, smallSideMargin, smallMargin);

            BackgroundColor = isBot ? gray : powelBlue;
            TextColor = isBot ? darkGray : Color.White;
        }

        public bool IsImageVisible => Image != null;

        public bool IsTextVisible => Image == null;

        public Thickness Margin { get; set; }

        public Color BackgroundColor { get; set; }

        public string Text { get; set; }

        public Color TextColor { get; set; }

        public ImageSource Image { get; set; }

        public bool Equals(ChatBubbleViewModel other)
        {
            return _id == other._id;
        }
    }
}