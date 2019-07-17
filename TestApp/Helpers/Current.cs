using System.Linq;
using TestApp.Model;
using TestApp.ViewModel;

namespace TestApp.Helpers
{
    internal static class Current
    {
        public static async void SetProfile(Profile profile)
        {
            Profile = profile;
        }

        /// <summary>
        /// Current logged profile.
        /// </summary>
        public static Profile Profile { get; private set; }

        /// <summary>
        /// Current view model being displayed.
        /// </summary>
        public static BaseViewModel ViewModel { get; set; }
    }
}