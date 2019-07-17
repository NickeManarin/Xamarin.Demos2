using Android.Content;
using TestApp.Controls;
using TestApp.Droid.Renderers;
using TestApp.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HyperlinkLabel), typeof(HyperlinkLabelRenderer))]
namespace TestApp.Droid.Renderers
{
    public class HyperlinkLabelRenderer : LabelRenderer
    {
        public HyperlinkLabelRenderer(Context context) : base(context)
        { }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                //Set TextView underlining.
                Control.PaintFlags |= Android.Graphics.PaintFlags.UnderlineText;
            }
        }
    }
}