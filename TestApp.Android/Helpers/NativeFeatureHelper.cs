using System.Threading.Tasks;
using Android.Content;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Locations;
using TestApp.Droid.Helpers;
using Plugin.CurrentActivity;
using TestApp.Helpers.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(NativeFeatureHelper))]
namespace TestApp.Droid.Helpers
{
    public class NativeFeatureHelper : INativeFeature
    {
        /// <summary>
        /// Task completion source used to detect the callback of the action of enabling the location services. 
        /// </summary>
        internal static TaskCompletionSource<bool> Cts;

        public bool IsLocationEnabled()
        {
            var manager = (LocationManager)CrossCurrentActivity.Current.AppContext.GetSystemService(Context.LocationService);
            return manager.IsProviderEnabled(LocationManager.GpsProvider);
        }

        public async Task<bool> TurnOnLocationSettings()
        {
            try
            {
                Cts = new TaskCompletionSource<bool>();

                var client = new GoogleApiClient.Builder(CrossCurrentActivity.Current.Activity).AddApi(LocationServices.API).Build();
                client.Connect();

                var locationRequest = LocationRequest.Create();
                locationRequest.SetPriority(LocationRequest.PriorityBalancedPowerAccuracy);
                locationRequest.SetInterval(30 * 1000);
                locationRequest.SetFastestInterval(5 * 1000);

                var builder = new LocationSettingsRequest.Builder().AddLocationRequest(locationRequest);
                //builder.SetAlwaysShow(true);

                //CrossCurrentActivity.Current.ActivityStateChanged += (sender, args) =>
                //{};

                var result = await LocationServices.SettingsApi.CheckLocationSettingsAsync(client, builder.Build());

                if (result.Status.StatusCode == CommonStatusCodes.Success)
                    return true;

                if (result.Status.StatusCode == CommonStatusCodes.ResolutionRequired)
                    result.Status.StartResolutionForResult(CrossCurrentActivity.Current.Activity, 0x1); //REQUEST_CHECK_SETTINGS
                else
                {
                    var settingIntent = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);
                    CrossCurrentActivity.Current.Activity.StartActivity(settingIntent);
                    return false;
                }
            }
            catch
            {
                return false;
            }

            return await Cts.Task;
        }
    }
}