using Android.OS;
using Android.Views;
using TestApp.Droid.Helpers;
using Plugin.CurrentActivity;
using TestApp.Helpers.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(NativeThemeHelper))]
namespace TestApp.Droid.Helpers
{
    public class NativeThemeHelper : INativeTheme
    {
        public void TopBarColor(Color color, int theme)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
                return;

            CrossCurrentActivity.Current.Activity.Window.SetStatusBarColor(color.ToAndroid());

            if (Build.VERSION.SdkInt < BuildVersionCodes.M)
                return;

            CrossCurrentActivity.Current.Activity.Window.DecorView.SystemUiVisibility = 
                theme == 1 ? 0 : (StatusBarVisibility)(SystemUiFlags.LightStatusBar | SystemUiFlags.LightNavigationBar);
        }

        public void BottomBarColor(Color color, int theme)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
                return;

            CrossCurrentActivity.Current.Activity.Window.SetNavigationBarColor(color.ToAndroid());
        }

        public Thickness GetBarSize()
        {
            var height = CrossCurrentActivity.Current.Activity.Window.DecorView.MeasuredHeight;

            int resourceId = CrossCurrentActivity.Current.Activity.Resources.GetIdentifier("status_bar_height", "dimen", "android");
            int resourceId1 = CrossCurrentActivity.Current.Activity.Resources.GetIdentifier("navigation_bar_height", "dimen", "android");

            var height2 = resourceId > 0 ? CrossCurrentActivity.Current.Activity.Resources.GetDimensionPixelSize(resourceId) : -1;
            var height3 = resourceId1 > 0 ? CrossCurrentActivity.Current.Activity.Resources.GetDimensionPixelSize(resourceId1) : -1;

            return new Thickness(0, height2, 0, height3);
        }
    }
}