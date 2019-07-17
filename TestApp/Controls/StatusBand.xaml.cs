using System;
using System.Threading.Tasks;
using TestApp.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestApp.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatusBand : ContentView
    {
        public static readonly BindableProperty ImageProperty = BindableProperty.Create("Image", typeof(ImageSource), typeof(StatusBand));

        public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(StatusBand), "...");

        public static readonly BindableProperty ModeProperty = BindableProperty.Create("Mode", typeof(Enums.StatusBandMode), typeof(StatusBand), Enums.StatusBandMode.Info, BindingMode.Default, null, Mode_PropertyChanged);


        public ImageSource Image
        {
            get => (ImageSource)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public Enums.StatusBandMode Mode
        {
            get => (Enums.StatusBandMode)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }

        public StatusBand()
        {
            InitializeComponent();

            BindingContext = this;
        }

        private static void Mode_PropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var band = bindable as StatusBand;
            var mode = newvalue as Enums.StatusBandMode?;

            if (!mode.HasValue)
                return;

            switch (mode.Value)
            {
                case Enums.StatusBandMode.Sucess:
                    band?.SetDynamicResource(BackgroundColorProperty, "Color.StatusBand.Sucess");
                    break;
                default:
                case Enums.StatusBandMode.Info:
                    band?.SetDynamicResource(BackgroundColorProperty, "Color.StatusBand.Info");
                    break;
                case Enums.StatusBandMode.Warning:
                    band?.SetDynamicResource(BackgroundColorProperty, "Color.StatusBand.Warning");
                    break;
                case Enums.StatusBandMode.Error:
                    band?.SetDynamicResource(BackgroundColorProperty, "Color.StatusBand.Error");
                    break;
            }
        }

        private void Hide_Clicked(object sender, EventArgs e)
        {
            Hide();
        }


        #region Public methods

        public async Task ShowInfo(string text)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Mode = Enums.StatusBandMode.Info;
                Text = text;
                IsVisible = true;
            });

            await this.FadeTo(1, 200);
        }

        public async Task ShowWarning(string text)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Mode = Enums.StatusBandMode.Warning;
                Text = text;
                IsVisible = true;
            });

            await this.FadeTo(1, 200);
        }

        public async Task ShowError(string text)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Mode = Enums.StatusBandMode.Error;
                Text = text;
                IsVisible = true;
            });

            await this.FadeTo(1, 200);
        }

        public void Hide()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Opacity = 0;
                IsVisible = false;
            });
        }

        #endregion
    }
}