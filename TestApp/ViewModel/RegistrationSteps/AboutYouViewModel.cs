using TestApp.Helpers;
using Xamarin.Forms;

namespace TestApp.ViewModel.RegistrationSteps
{
    internal class AboutYouViewModel : BaseViewModel
    {
        private string _about;

        public string About
        {
            get => _about;
            set => SetProperty(ref _about, value);
        }

        public AboutYouViewModel()
        {
        }

        internal bool IsValid()
        {
            return true;
        }
    }
}