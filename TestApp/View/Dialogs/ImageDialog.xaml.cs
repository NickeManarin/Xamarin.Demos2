using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using TestApp.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestApp.View.Dialogs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageDialog
    {
        public ImageDialog()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            InfoPancake.FadeTo(1, 300, Easing.SinIn);
            MainPopupPage.BackgroundColorTo(Color.Transparent, ResourceHelper.TryGetColor("Color.Background.Faded", Color.Transparent), 300, Easing.SinOut);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            InfoPancake.FadeTo(0, 300, Easing.SinOut);
            MainPopupPage.BackgroundColorTo(ResourceHelper.TryGetColor("Color.Background.Faded", Color.Transparent), Color.Transparent, 300, Easing.SinOut);
        }

        internal void FocusOnElement()
        {
            TakeButton.Focus();
        }

        #region Events

        public event EventHandler<bool?> Picked;

        private void Take_Clicked(object sender, EventArgs e)
        {
            Picked?.Invoke(this, true);
        }

        private void Pick_Clicked(object sender, EventArgs e)
        {
            Picked?.Invoke(this, false);
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            Picked?.Invoke(this, null);
        }

        #endregion

        /// <summary>
        /// Opens up a pop up with two options.
        /// </summary>
        /// <returns>
        /// True if the camera mode was selected.
        /// False if the pick image mode was selected.
        /// Null if no option was selected.
        /// </returns>
        public static Task<bool?> SelectImageMode()
        {
            var cts = new TaskCompletionSource<bool?>();

            var view = new ImageDialog();
            view.FocusOnElement();
            view.Picked += (s, o) =>
            {
                cts.SetResult(o);
                PopupNavigation.Instance.PopAsync();
            };

            PopupNavigation.Instance.PushAsync(view);

            return cts.Task;
        }
    }
}