using System.Collections.Generic;
using System.Threading.Tasks;
using DLToolkit.Forms.Controls;
using TestApp.Helpers.Interfaces;
using TestApp.Model;
using TestApp.ViewModel.RegistrationSteps;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestApp.View.RegistrationSteps
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InterestsView : ContentView, INavigationStepper<Profile>
    {
        internal InterestsViewModel ViewModel { get; } = new InterestsViewModel();

        public InterestsView()
        {
            InitializeComponent();

#if DEBUG
            ViewModel.InterestList = new FlowObservableCollection<Interest>
            {
                new Interest{ Content = "tag1", Language = "en", WasBanned = true },
                new Interest{ Content = "other", Language = "en" }
            };
#endif
            BindingContext = ViewModel;
        }

        public Profile Transient { get; set; }
        public double Progress => 0.6;
        public bool IsSkippable { get; set; } = false;
        public bool IsInteractive { get; set; } = true;
        public string Header => "Tags";
        public string ActionTitle { get; set; } = "Continue";

        public void Loaded()
        {
            if (Transient.Interests != null)
                foreach (var interest in Transient.Interests)
                    ViewModel.InterestList.Add(interest);
        }

        public void Refresh()
        { }

        public void Unload()
        { }

        public INavigationStepper<Profile> Previous()
        {
            return new PhotosView { Transient = Transient };
        }

        public async Task<bool> IsValid()
        {
            return await Task.FromResult(ViewModel.IsValid());
        }

        public Profile FillIn(Profile profile)
        {
            profile.Interests = new List<Interest>();

            foreach (var interest in ViewModel.InterestList)
                profile.Interests.Add(interest);

            return profile;
        }

        public INavigationStepper<Profile> Next()
        {
            Transient.Interests = new List<Interest>();

            foreach (var interest in ViewModel.InterestList)
                Transient.Interests.Add(interest);

            return null;
        }
    }
}