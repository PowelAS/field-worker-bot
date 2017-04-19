using System;
using Powel.BotClient.ViewModels;
using Xamarin.Forms;

namespace Powel.BotClient
{
    public partial class MainPage : ContentPage
    {
        private bool _wasInitialized;

        private MainPageViewModel _mainPageViewModel;

        public MainPage()
        {
            _mainPageViewModel = new MainPageViewModel(() => Scroll.ScrollToAsync(0, 999999, false));
            BindingContext = _mainPageViewModel;

            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!_wasInitialized)
            {
                _wasInitialized = true;

                await _mainPageViewModel.StartBotCommunication();
            }
        }

        private async void OnSendTextButtonClicked(object sender, EventArgs e)
        {
            await _mainPageViewModel.EnterPressed();
        }
    }
}
