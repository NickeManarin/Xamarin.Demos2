using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TestApp.Helpers;

namespace TestApp.ViewModel
{
    public class BaseViewModel : BindableBase
    {
        private string _title = string.Empty;
        private string _subTitle = string.Empty;
        private string _icon;
        private bool _isBusy;
        private string _errorMessage;

        protected Xamarin.Forms.View View;

        public BaseViewModel()
        {
            Current.ViewModel = this;
        }

        /// <summary>
        /// Gets or sets the "Title" property of the view.
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        /// <summary>
        /// Gets or sets the "Subtitle" property of the view.
        /// </summary>
        public string Subtitle
        {
            get => _subTitle;
            set => SetProperty(ref _subTitle, value);
        }

        /// <summary>
        /// Gets or sets the "Icon" of the view.
        /// </summary>
        public string Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        /// <summary>
        /// Gets or sets if the view is busy or not.
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                SetProperty(ref _isBusy, value);
                OnPropertyChanged(nameof(IsWorking));
                OnPropertyChanged(nameof(Opacity));
            }
        }

        public bool IsWorking => !_isBusy;

        public double Opacity => _isBusy ? 0.7 : 1;


        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

        /// <summary>
        /// The error message.
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetProperty(ref _errorMessage, value);
                OnPropertyChanged(nameof(HasError));
            }
        }

        public async Task RunSafe(Task task, bool showLoading = true, string loadingMessage = null)
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                //if (ShowLoading)
                //    UserDialogs.Instance.ShowLoading(loadingMessage ?? "Loading");

                await task;
            }
            catch (Exception e)
            {
                IsBusy = false;
                //UserDialogs.Instance.HideLoading();
                Debug.WriteLine(e.ToString());
                //await App.Current.MainPage.DisplayAlert("Eror", "Check your internet connection", "Ok");
            }
            finally
            {
                IsBusy = false;

                //if (ShowLoading)
                //    UserDialogs.Instance.HideLoading();
            }
        }
    }
}