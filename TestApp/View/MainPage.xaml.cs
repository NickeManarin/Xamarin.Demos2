using TestApp.Controls;
using TestApp.ViewModel;
using Xamarin.Forms.Xaml;

namespace TestApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ExtendedContentPage
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();

            BindingContext = ViewModel;
        }

        protected override void OnFirstAppearing()
        {
            base.OnFirstAppearing();

            ViewModel.VerifyLoginCommand.Execute(null);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}