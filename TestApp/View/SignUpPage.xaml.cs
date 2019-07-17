using TestApp.Controls;
using TestApp.Helpers;
using TestApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ExtendedContentPage
    {
        public SignUpViewModel ViewModel { get; } = new SignUpViewModel();

        public SignUpPage()
        {
            InitializeComponent();

            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            MessagingCenter.Subscribe<BaseViewModel, string>(this, Constants.ShowInfo, async (view, param) => await StatusBand.ShowInfo(param));
            MessagingCenter.Subscribe<BaseViewModel, string>(this, Constants.ShowWarning, async (view, param) => await StatusBand.ShowWarning(param));
            MessagingCenter.Subscribe<BaseViewModel, string>(this, Constants.ShowError, async (view, param) => await StatusBand.ShowError(param));
            MessagingCenter.Subscribe<BaseViewModel>(this, Constants.Hide, view => StatusBand.Hide());
            MessagingCenter.Subscribe<BaseViewModel>(this, Constants.NavigateNext, view => ViewModel.NextCommand.Execute(null));
            MessagingCenter.Subscribe<BaseViewModel>(this, Constants.NavigateNextForced, view => ViewModel.NextForcedCommand.Execute(null));
            MessagingCenter.Subscribe<BaseViewModel>(this, Constants.NavigateTo, view => ViewModel.NavigateToCommand.Execute(null));

            ViewModel.LoadCommand.Execute(null);

            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<BaseViewModel, string>(this, Constants.ShowInfo);
            MessagingCenter.Unsubscribe<BaseViewModel, string>(this, Constants.ShowWarning);
            MessagingCenter.Unsubscribe<BaseViewModel, string>(this, Constants.ShowError);
            MessagingCenter.Unsubscribe<BaseViewModel>(this, Constants.Hide);
            MessagingCenter.Unsubscribe<BaseViewModel>(this, Constants.NavigateNext);
            MessagingCenter.Unsubscribe<BaseViewModel>(this, Constants.NavigateNextForced);
            MessagingCenter.Unsubscribe<BaseViewModel>(this, Constants.NavigateTo);

            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            if (!ViewModel.CurrentView.IsInteractive)
                return true;

            ViewModel.BackCommand.Execute(null);

            //Disables the back navigation in order to control the navigation.
            return true;
        }

        internal override void OnResume()
        {
            ViewModel?.CurrentView?.Refresh();

            base.OnResume();
        }
    }
}