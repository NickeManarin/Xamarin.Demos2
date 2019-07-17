using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using Xamarin.Forms;

namespace TestApp.Helpers
{
    internal class LocalizationHelper
    {
        internal static void SelectCulture(string culture)
        {
            #region Validation

            //If none selected, fallback to english.
            if (string.IsNullOrEmpty(culture))
                culture = "en";

            if (culture.Equals("auto") || culture.Length < 2)
            {
                var ci = CultureInfo.InstalledUICulture;
                culture = ci.Name;
            }

            #endregion

            //Copy all MergedDictionarys into a auxiliar list.
            var dictionaryList = Application.Current.Resources.MergedDictionaries.ToList();

            #region Selected Culture

            //Search for the specified culture.
            var requestedCulture = $"Resources/Localizations/Strings_{culture}.xaml";
            var requestedResource = dictionaryList.FirstOrDefault(d => d.Source?.OriginalString == requestedCulture);

            #endregion

            #region Generic Branch Fallback

            //Fallback to a more generic version of the language. Example: pt-BR to pt.
            while (requestedResource == null && !string.IsNullOrEmpty(culture))
            {
                culture = CultureInfo.GetCultureInfo(culture).Parent.Name;
                requestedCulture = $"Resources/Localizations/StringResources.{culture}.xaml";
                requestedResource = dictionaryList.FirstOrDefault(d => d.Source?.OriginalString == requestedCulture);
            }

            #endregion

            #region English Fallback

            //If not present, fall back to english.
            if (requestedResource == null)
            {
                culture = "en";
                requestedCulture = "Resources/Localizations/Strings_en.xaml";
                requestedResource = dictionaryList.FirstOrDefault(d => d.Source?.OriginalString == requestedCulture);
            }

            #endregion

            #region English Fallback of the Current Language

            //Only non-English resources need a fallback, because the English resource is evergreen.
            if (!culture.StartsWith("en"))
            {
                var englishResource = dictionaryList.FirstOrDefault(d => d.Source?.OriginalString == "Resources/Localizations/Strings_en.xaml");

                if (englishResource != null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(englishResource);
                    Application.Current.Resources.MergedDictionaries.Add(englishResource);
                    //Application.Current.Resources.MergedDictionaries.Insert(Application.Current.Resources.MergedDictionaries.Count - 1, englishResource);
                }
            }

            #endregion

            //If we have the requested resource, remove it from the list and place at the end.
            //Then this language will be our current string table.
            Application.Current.Resources.MergedDictionaries.Remove(requestedResource);
            Application.Current.Resources.MergedDictionaries.Add(requestedResource);

            //Inform the threads of the new culture.
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

            GC.Collect(0);
        }

        internal static string Get(string key)
        {
            return ResourceHelper.TryGetString(key, "");
        }
    }
}