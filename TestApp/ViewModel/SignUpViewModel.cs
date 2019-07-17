using System;
using System.Threading.Tasks;
using TestApp.Helpers;
using TestApp.Helpers.Interfaces;
using TestApp.Model;
using TestApp.View.RegistrationSteps;
using Xamarin.Forms;

namespace TestApp.ViewModel
{
    public class SignUpViewModel : BaseViewModel
    {
        private INavigationStepper<Profile> _currentView;

        public Command LoadCommand { get; set; }
        public Command BackCommand { get; set; }
        public Command NextCommand { get; set; }
        public Command NextForcedCommand { get; set; }
        public Command<string> NavigateToCommand { get; set; }

        public INavigationStepper<Profile> CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public SignUpViewModel()
        {
            Title = "Sign Up";

            LoadCommand = new Command(Load_Executed);
            BackCommand = new Command(async () => await Back_Executed());
            NextCommand = new Command(async () => await Next_Executed());
            NextForcedCommand = new Command(async () => await ForceNext_Executed());
            NavigateToCommand = new Command<string>(NavigateBackTo_Executed);

            IsBusy = true;
        }

        private void Load_Executed()
        {
            CurrentView = new AboutYouView { Transient = new Profile { TypeOfProfile = Profile.Corporative } };
            //CurrentView = new InterestsView { Transient = new Profile { TypeOfProfile = Profile.Corporative } };
            CurrentView?.Loaded();

            IsBusy = false;
        }

        private async Task Next_Executed()
        {
            if (IsBusy)
                return;

            await ForceNext_Executed();
        }

        private async Task ForceNext_Executed()
        {
            try
            {
                IsBusy = true;

                MessagingCenter.Send<BaseViewModel>(this, Constants.Hide);

                if (!await CurrentView.IsValid())
                    return;

                var next = CurrentView.Next();
                CurrentView.Unload();

                if (next != null)
                {
                    next.Loaded();
                    CurrentView = next;
                    return;
                }

                //Navigate back to the main page.
                await App.Navigation.GoToStartAsync(true);
            }
            catch (Exception e)
            {
                MessagingCenter.Send<BaseViewModel, string>(this, Constants.ShowWarning, e.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task Back_Executed()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                MessagingCenter.Send<BaseViewModel>(this, Constants.Hide);

                var previous = CurrentView.Previous();
                CurrentView.Unload();

                //Return to the previous view, if there's any.
                if (previous != null)
                {
                    previous.Loaded();
                    CurrentView = previous;
                    return;
                }

                //If not, return to the Startup page (or the Main page if the profile creation was triggered there).
                await App.Navigation.GoBackAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void NavigateBackTo_Executed(string param)
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                //MessagingCenter.Send<BaseViewModel>(this, Constants.Hide);

                //Navigate directly to the view with the faulty detail that needs to be fixed.
                CurrentView = CurrentView.Previous();
            }
            catch (Exception e)
            {
                //Display or not? Because this command will be fired when 
                MessagingCenter.Send<BaseViewModel, string>(this, Constants.ShowWarning, e.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}