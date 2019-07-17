using System;
using System.Diagnostics;
using System.Linq;
using TestApp.Helpers.Interfaces;
using TestApp.Model;
using Xamarin.Forms;

namespace TestApp.Helpers
{
    internal static class ThemeHelper
    {
        /// <summary>
        /// Selects the light or dark themes.
        /// </summary>
        /// <param name="theme">The id of the theme to be selected, 0 is light and 1 is dark.</param>
        internal static void SelectTheme(int theme)
        {
            try
            {
                #region Validation

                //If none selected, fallback to Light mode.
                if (theme < 0 || theme > 1)
                    theme = 0;

                #endregion

                //Copy all MergedDictionarys into a auxiliar list.
                var dictionaryList = Application.Current.Resources.MergedDictionaries.ToList();

                #region Selected Theme

                //Search for the specified culture.
                var requestedCulture = $"Resources/Themes/{ThemeIdToName(theme)}.xaml";
                var requestedResource = dictionaryList.FirstOrDefault(d => d.Source?.OriginalString == requestedCulture);

                #endregion

                //If we have the requested resource, remove it from the list and place at the end.
                //Then this theme will be our current style table.
                Application.Current.Resources.MergedDictionaries.Remove(requestedResource);
                Application.Current.Resources.MergedDictionaries.Add(requestedResource);

                GC.Collect(0);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                //Change the color of the Top and Bottom system bars, if available.
                DependencyService.Get<INativeTheme>().TopBarColor(ResourceHelper.TryGetColor("Color.Background.Even", Color.DarkBlue), theme);
                DependencyService.Get<INativeTheme>().BottomBarColor(ResourceHelper.TryGetColor("Color.Background.Odd", Color.DarkBlue), theme);
            }
        }

        internal static string ThemeIdToName(int id)
        {
            switch (id)
            {
                case Options.LightTheme:
                    return "Light";
                case Options.DarkTheme:
                    return "Dark";
                default:
                    return null;
            }
        }
    }
}