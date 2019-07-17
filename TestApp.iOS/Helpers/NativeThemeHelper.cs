using TestApp.iOS.Helpers;
using TestApp.Helpers.Interfaces;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(NativeThemeHelper))]
namespace TestApp.iOS.Helpers
{
    public class NativeThemeHelper : INativeTheme
    {
        public void TopBarColor(Color color, int theme)
        {
            UIApplication.SharedApplication.StatusBarStyle = theme == 1 ? UIStatusBarStyle.LightContent : UIStatusBarStyle.BlackTranslucent;
        }

        public void BottomBarColor(Color color, int theme)
        {
            //Nothing can be done.
        }
    }
}