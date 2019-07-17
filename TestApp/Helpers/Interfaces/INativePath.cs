namespace TestApp.Helpers.Interfaces
{
    public interface INativePath
    {
        string GetLocalFilePath(string filename);
        string GetDatabaseFilePath(string filename);
        string GetCustomFilePath(string folder, string filename);
        string GetDownloadsFolder();
    }
}