using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApp.Helpers.Interfaces;
using TestApp.Model;
using TestApp.ViewModel.RegistrationSteps;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Image = TestApp.Model.Image;

namespace TestApp.View.RegistrationSteps
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotosView : ContentView, INavigationStepper<Profile>
    {
        internal PhotosViewModel ViewModel { get; } = new PhotosViewModel();

        public PhotosView()
        {
            InitializeComponent();

            BindingContext = ViewModel;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width < 10)
                return;

            MainGrid.Padding = new Thickness(0);
            MainListView.ItemWidth = (int)((width * 0.90) / 3d);
            MainListView.ItemHeight = (int)(MainListView.ItemWidth * 1.40d);
            MainListView.WidthRequest = width * 0.90; //90% of the width.
            MainListView.HeightRequest = (MainListView.ItemHeight + MainListView.ItemSpacing) * 2;

            var req = MainListView.Measure(double.PositiveInfinity, double.PositiveInfinity);
            var req2 = MainGrid.Measure(double.PositiveInfinity, double.NegativeInfinity);

            var widthAvailable = width - req2.Request.Width;
            var heightAvailable = height - req2.Request.Height - PhotosLabel.Height - PhotosLabel.Margin.VerticalThickness;

            MainGrid.Padding = new Thickness(widthAvailable / 2d, heightAvailable / 2d, widthAvailable / 2d, heightAvailable / 2d);
        }

        public Profile Transient { get; set; }
        public double Progress => Transient?.TypeOfProfile == Profile.Corporative ?  0.6 : 0.5;
        public bool IsSkippable { get; set; } = false;
        public bool IsInteractive { get; set; } = true;
        public string Header => Transient?.TypeOfProfile == Profile.Corporative ? "Let your company be seen" : "Show us who you are";
        public string ActionTitle { get; set; } = "Continue";

        public void Loaded()
        {
            //MessagingCenter.Subscribe<BaseViewModel>(this, Constants.RefreshInterface, view =>
            //{
            //    MainListView.IsVisible = false;

            //    MainListView.CheckConsistency();

            //    MainListView.HeightRequest = -1;
            //    var req = MainListView.Measure(double.PositiveInfinity, double.NegativeInfinity);
            //    MainListView.HeightRequest = req.Request.Height;

            //    if (req.Request.Height < 0)
            //    {
            //        var rows = Math.Ceiling(ViewModel.Items.Count / (double)MainListView.ColumnCount);
            //        MainListView.HeightRequest = rows * (MainListView.ItemHeight + MainListView.ItemSpacing);
            //    }

            //    MainListView.IsVisible = true;
            //});

            ViewModel.LoadPhotosCommand.Execute(Transient?.Images);
        }

        public void Refresh()
        { }

        public void Unload()
        {
            //MessagingCenter.Unsubscribe<BaseViewModel>(this, Constants.RefreshInterface);
        }

        public INavigationStepper<Profile> Previous()
        {
            return new AboutYouView { Transient = Transient };
        }

        public async Task<bool> IsValid()
        {
            //Alters the profile mode for validation.
            ViewModel.IsPersonal = Transient.IsPersonal;

            return await Task.Factory.StartNew(() => ViewModel.IsValid());
        }

        public Profile FillIn(Profile profile)
        {
            Transient.Images = new List<Image>();

            foreach (var image in ViewModel.Items.Where(w => w.HasImage))
                profile.Images.Add(image);

            return profile;
        }

        public INavigationStepper<Profile> Next()
        {
            Transient.Images = new List<Image>();

            foreach (var image in ViewModel.Items.Where(w => w.HasImage))
                Transient.Images.Add(image);

            return new InterestsView { Transient = Transient };
        }

        private void MainListView_ChildrenReordered(object sender, EventArgs e)
        {
            for (var i = 0; i < ViewModel.Items.Count; i++)
                ViewModel.Items[i].Position = i + 1;
        }
    }
}