using TestApp.View;
using Xamarin.Forms;

namespace TestApp.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public Command VerifyLoginCommand { get; set; }

        public MainViewModel()
        {
            Title = "Main";

            VerifyLoginCommand = new Command(VerifyLogin_Executed);
        }

        private async void VerifyLogin_Executed(object obj)
        {
            IsBusy = true;

            await App.Navigation.NavigateModalAsync(new StartupPage(), false);

            IsBusy = false;
        }
    }
}