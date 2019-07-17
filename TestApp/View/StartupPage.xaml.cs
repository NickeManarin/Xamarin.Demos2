using TestApp.Controls;
using TestApp.ViewModel;
using Xamarin.Forms.Xaml;

namespace TestApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartupPage : ExtendedContentPage
    {
        public StartupViewModel ViewModel { get; } = new StartupViewModel();

        public StartupPage()
        {
            InitializeComponent();

#if DEBUG

#endif
            BindingContext = ViewModel;
        }

        protected override bool OnBackButtonPressed()
        {
            //Disables the back navigation.
            //Maybe ask to close?
            return true;
        }
    }
}