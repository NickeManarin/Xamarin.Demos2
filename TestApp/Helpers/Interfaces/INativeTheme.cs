using Xamarin.Forms;

namespace TestApp.Helpers.Interfaces
{
    public interface INativeTheme
    {
        void TopBarColor(Color color, int theme);
        void BottomBarColor(Color color, int theme);
    }
}