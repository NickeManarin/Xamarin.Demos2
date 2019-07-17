using System;
using Xamarin.Forms;

namespace TestApp.Controls
{
    public class HyperlinkLabel : Label
    {
        public static readonly BindableProperty UriProperty = BindableProperty.Create(nameof(Uri), typeof(string), typeof(HyperlinkLabel), null);

        public string Uri
        {
            get => (string)GetValue(UriProperty);
            set => SetValue(UriProperty, value);
        }

        public HyperlinkLabel()
        {
            TextColor = Color.Accent;

            //Underlining is set by custom renderers.
            //On Android and UWP only, as it is against the iOS design guidelines.

            //Interaction.
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (sender, args) =>
            {
                if (Uri != null)
                    Device.OpenUri(new Uri(Uri));
            };

            GestureRecognizers.Add(tapGestureRecognizer);
        }
    }
}