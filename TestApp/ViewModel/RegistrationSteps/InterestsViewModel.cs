using System;
using DLToolkit.Forms.Controls;
using TestApp.Helpers;
using TestApp.Model;
using Xamarin.Forms;

namespace TestApp.ViewModel.RegistrationSteps
{
    internal class InterestsViewModel : BaseViewModel
    {
        private FlowObservableCollection<Interest> _interestList = new FlowObservableCollection<Interest>();
        private string _newInterest;
        private string _appLanguage;

        public FlowObservableCollection<Interest> InterestList
        {
            get => _interestList;
            set => SetProperty(ref _interestList, value);
        }

        public string NewInterest
        {
            get => _newInterest;
            set => SetProperty(ref _newInterest, value);
        }

        public string AppLanguage
        {
            get => _appLanguage;
            set => SetProperty(ref _appLanguage, value);
        }

        public Command RemoveInterestCommand { get; set; }
        public Command AddInterestCommand { get; set; }

        public bool IsInterestListEmpty => (InterestList?.Count ?? 0) == 0;


        public InterestsViewModel()
        {
            Title = "Interests";

            RemoveInterestCommand = new Command<Interest>(RemoveInterest_Executed);
            AddInterestCommand = new Command<Interest>(AddInterest_Executed);
        }


        private void RemoveInterest_Executed(Interest interest)
        {
            try
            {
                IsBusy = true;

                if (interest == null)
                    return;

                InterestList.Remove(interest);
            }
            catch (Exception)
            {
                MessagingCenter.Send<BaseViewModel, string>(this, Constants.ShowError, "It was not possible to remove the tag.");
            }
            finally
            {
                IsBusy = false;
                OnPropertyChanged(nameof(IsInterestListEmpty));
            }
        }

        private void AddInterest_Executed(Interest interest)
        {
            try
            {
                IsBusy = true;

                MessagingCenter.Send<BaseViewModel>(this, Constants.Hide);

                //If the param is null, it means that the user is typing the interest manually instead of picking on from the list.
                if (interest == null)
                {
                    //Remove any spaces (from the start and end) and # (from the start).
                    NewInterest = (NewInterest ?? "").Trim().TrimStart('#').Trim();

                    if (!IsTagValid(NewInterest))
                        return;

                    InterestList.Add(new Interest
                    {
                        Content = NewInterest,
                        Language = AppLanguage ?? "en"
                    });
                    NewInterest = "";

                    return;
                }

                InterestList.Add(interest);
            }
            catch (Exception e)
            {
                MessagingCenter.Send<BaseViewModel, string>(this, Constants.ShowError, "It was not possible to add the interest.");
            }
            finally
            {
                IsBusy = false;
                OnPropertyChanged(nameof(IsInterestListEmpty));
            }
        }

        internal bool IsTagValid(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                MessagingCenter.Send<BaseViewModel, string>(this, Constants.ShowWarning, "You need to type something to add.");
                return false;
            }

            return true;
        }

        internal bool IsValid()
        {
            if (InterestList.Count < 1)
            {
                MessagingCenter.Send<BaseViewModel, string>(this, Constants.ShowWarning, "Please, select or add at least 1 tag.");
                return false;
            }

            return true;
        }
    }
}