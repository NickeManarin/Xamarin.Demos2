using Xamarin.Forms;

namespace TestApp.Helpers
{
    internal static class ResourceHelper
    {
        internal static Color TryGetColor(string key, Color fallback)
        {
            Application.Current.Resources.TryGetValue(key, out var color);

            return color as Color? ?? fallback;
        }

        internal static string TryGetString(string key, string fallback)
        {
            Application.Current.Resources.TryGetValue(key, out var text);

            return text as string ?? fallback;
        }

        internal static Style TryGetStyle(string key, Style fallback = null)
        {
            Application.Current.Resources.TryGetValue(key, out var style);

            return style as Style ?? fallback;
        }
    }
}