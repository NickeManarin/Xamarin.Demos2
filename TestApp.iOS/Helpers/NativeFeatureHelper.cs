using System.Threading.Tasks;
using CoreLocation;
using Foundation;
using TestApp.iOS.Helpers;
using TestApp.Helpers.Interfaces;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(NativeFeatureHelper))]
namespace TestApp.iOS.Helpers
{
    public class NativeFeatureHelper : INativeFeature
    {
        public bool IsLocationEnabled()
        {
            return CLLocationManager.Status != CLAuthorizationStatus.Denied;
        }

        public async Task<bool> TurnOnLocationSettings()
        {
            var wiFiUrl = new NSUrl("prefs:root=LOCATION_SERVICES");

            if (UIApplication.SharedApplication.CanOpenUrl(wiFiUrl))
                UIApplication.SharedApplication.OpenUrl(wiFiUrl); //Pre iOS 10.
            else
                await UIApplication.SharedApplication.OpenUrlAsync(new NSUrl("App-Prefs:root=Privacy&path=LOCATION_SERVICES"), new UIApplicationOpenUrlOptions()); //iOS 10.

            //UIApplication.OpenSettingsUrlString();

            return false;
        }
    }
}