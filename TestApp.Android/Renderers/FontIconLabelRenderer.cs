using Android.Content;
using Android.Graphics;
using TestApp.Controls;
using TestApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

//[assembly: ExportRenderer(typeof(FontIconLabel), typeof(FontIconLabelRenderer))]
namespace TestApp.Droid.Renderers
{
    public class FontIconLabelRenderer : LabelRenderer
    {
        public FontIconLabelRenderer(Context context) : base(context)
        { }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
                Control.Typeface = Typeface.CreateFromAsset(Context.Assets, FontIconLabel.FontName);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "Text")
                Control.Typeface = Typeface.CreateFromAsset(Context.Assets, FontIconLabel.FontName);
        }
    }
}