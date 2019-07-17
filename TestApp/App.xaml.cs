using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DLToolkit.Forms.Controls;
using Microsoft.EntityFrameworkCore;
using TestApp.Controls;
using TestApp.Helpers;
using TestApp.Helpers.Interfaces;
using TestApp.Model;
using TestApp.View;
using Xamarin.Forms;

namespace TestApp
{
    public partial class App : Application
    {
        internal static readonly NavigationHelper Navigation = new NavigationHelper();

        public App()
        {
            InitializeComponent();

            FlowListView.Init();
            
            Navigation.Start(new MainPage());
        }

        protected override void OnStart()
        {
            LocalizationHelper.SelectCulture("en");
            ThemeHelper.SelectTheme(0);
        }

        protected override void OnSleep()
        {
            //Handle when your app sleeps.
        }

        protected override void OnResume()
        {
            //Fires the OnResume on supported pages.
            //So we can detect any changes in the system settings while the app was on sleep.

            if (Navigation.CurrentPage.CurrentPage is ExtendedContentPage page)
                page.OnResume();
            else if (Navigation.CurrentPage.CurrentPage is ExtendedPopupPage dialog)
                dialog.OnResume();
        }
    }
}