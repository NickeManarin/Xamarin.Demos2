using Xamarin.Forms;

namespace TestApp.Controls
{
    public class FontIconLabel : Label
    {
        public static readonly string FullFontName = Device.RuntimePlatform == "iOS" ? "MatchIcons" : "Fonts/match_icons.ttf#MatchIcons";
        public static readonly string FontName = Device.RuntimePlatform == "iOS" ? "MatchIcons" : "Fonts/match_icons.ttf";

        public FontIconLabel()
        {
            //FontFamily = FontName;
        }

        public FontIconLabel(string text = null) : this()
        {
            Text = text;
        }
    }

    public static class Icon
    {
        //Via code: \uE98F
        //Via Xaml: &#xE98F;
        //Convert Xaml notation to char: System.Net.WebUtility.HtmlDecode("&#xe98f;")

        public static readonly string Location = "" + '\uE98F';
        public static readonly string Close = "" + '\uEA0E';
        public static readonly string Hash = "" + '\uE978';
        public static readonly string Plus = "" + '\uE9BA';
        public static readonly string Settings = "" + '\uE9CB';
    }
}