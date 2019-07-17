namespace TestApp.Helpers
{
    internal static class Extensions
    {
        internal static bool IsNullOrEmpty(this byte[] array) => array == null || array.Length == 0;
    }
}