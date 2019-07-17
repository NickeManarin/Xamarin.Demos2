using System;
using System.IO;
using TestApp.iOS.Helpers;
using TestApp.Helpers.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(NativePathHelper))]
namespace TestApp.iOS.Helpers
{
    public class NativePathHelper : INativePath
    {
        public string GetCustomFilePath(string folder, string filename)
        {
            var docFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var libFolder = Path.Combine(docFolder, "..", "Library", folder);

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
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return Path.Combine(documents, "..", "Library");
        }
    }
}