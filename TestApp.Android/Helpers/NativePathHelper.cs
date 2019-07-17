using System.IO;
using TestApp.Droid.Helpers;
using TestApp.Helpers.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(NativePathHelper))]
namespace TestApp.Droid.Helpers
{
    public class NativePathHelper : INativePath
    {
        public string GetCustomFilePath(string folder, string filename)
        {
            var docFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var libFolder = Path.Combine(docFolder, folder);

            if (!Directory.Exists(libFolder))
                Directory.CreateDirectory(libFolder);

            return Path.Combine(libFolder, filename);
        }

        public string GetDatabaseFilePath(string filename)
        {
            return GetCustomFilePath("Databases", filename);
        }

        public string GetLocalFilePath(string filename)
        {
            return GetCustomFilePath("Files", filename);
        }

        public string GetDownloadsFolder()
        {
            return Android.OS.Environment.DirectoryDownloads;
        }
    }
}