using System.Threading.Tasks;
using TestApp.Helpers.Interfaces;
using TestApp.Model;
using TestApp.ViewModel.RegistrationSteps;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestApp.View.RegistrationSteps
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutYouView : ContentView, INavigationStepper<Profile>
    {
        internal AboutYouViewModel ViewModel { get; } = new AboutYouViewModel();

        public AboutYouView()
        {
            InitializeComponent();

#if DEBUG
            ViewModel.About = "Test.";
#endif

            BindingContext = ViewModel;

            base.OnPropertyChanged(nameof(Transient));
        }

        public Profile Transient { get; set; }
        public double Progress => 0.4;
        public bool IsSkippable { get; set; } = false;
        public bool IsInteractive { get; set; } = true;
        public string Header => "Test";
        public string ActionTitle { get; set; } = "Continue";

        public void Loaded()
        { }

        public void Refresh()
        { }

        public void Unload()
        { }
        public INavigationStepper<Profile> Previous()
        {
            return null;
        }

        public async Task<bool> IsValid()
        {
            return await Task.Factory.StartNew(() => ViewModel.IsValid());
        }

        public Profile FillIn(Profile profile)
        {
            profile.About = ViewModel.About;

            return profile;
        }

        public INavigationStepper<Profile> Next()
        {
            return new PhotosView { Transient = Transient };
        }
    }
}