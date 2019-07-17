using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using TestApp.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestApp.View.Dialogs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuDialog
    {
        public MainMenuDialog()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            InfoPancake.FadeTo(1, 300, Easing.SinIn);
            MainMenuPopupPage.BackgroundColorTo(Color.Transparent, ResourceHelper.TryGetColor("Color.Background.Faded", Color.Transparent), 300, Easing.SinOut);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            InfoPancake.FadeTo(0, 300, Easing.SinOut);
            MainMenuPopupPage.BackgroundColorTo(ResourceHelper.TryGetColor("Color.Background.Faded", Color.Transparent), Color.Transparent, 300, Easing.SinOut);
        }

        internal void AttachBindings()
        {
            //BindingContext = new
            //{
            //};
        }


        #region Events

        public event EventHandler<int?> Picked;
        
        private void Options_Clicked(object sender, EventArgs e)
        {
            Picked?.Invoke(this, 0);
        }

        private void Logout_Clicked(object sender, EventArgs e)
        {
            Picked?.Invoke(this, 1);
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
        public static Task<int?> ShowMenu()
        {
            var cts = new TaskCompletionSource<int?>();

            var view = new MainMenuDialog();
            view.AttachBindings();
            view.Picked += (s, o) =>
            {
                PopupNavigation.Instance.PopAsync();
                cts.SetResult(o);
            };

            PopupNavigation.Instance.PushAsync(view);

            return cts.Task;
        }
    }
}