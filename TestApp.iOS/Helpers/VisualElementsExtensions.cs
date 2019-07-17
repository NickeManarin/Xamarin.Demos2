using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace TestApp.iOS.Helpers
{
    internal static class VisualElementsExtensions
    {
        internal static bool UseLegacyColorManagement<T>(this T element) where T : Xamarin.Forms.VisualElement, IElementConfiguration<T>
        {
            if (!element.HasVisualStateGroups())
                return element.OnThisPlatform<T>().GetIsLegacyColorModeEnabled();

            return false;
        }
    }
}