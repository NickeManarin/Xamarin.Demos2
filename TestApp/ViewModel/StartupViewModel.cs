using System.Threading.Tasks;
using TestApp.View;
using TestApp.View.Dialogs;
using Xamarin.Forms;

namespace TestApp.ViewModel
{
    public class StartupViewModel : BaseViewModel
    {
        public Command SignUpCommand { get; set; }
        public Command SignInCommand { get; set; }

        public StartupViewModel()
        {
            Title = "Start Up";

            SignUpCommand = new Command(async () => await SignUp_Executed());
            SignInCommand = new Command(async () => await SignIn_Executed());
        }

        #region Events

        private async Task SignUp_Executed()
        {
            await App.Navigation.NavigateModalAsync(new SignUpPage());
        }

        private async Task SignIn_Executed()
        {
            await MainMenuDialog.ShowMenu();
        }

        #endregion
    }
}