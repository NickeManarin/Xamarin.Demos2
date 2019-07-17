using CoreAnimation;
using TestApp.iOS.Renderers;
using TestApp.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedContentPage), typeof(ExtendedContentPageRenderer))]
namespace TestApp.iOS.Renderers
{
    class ExtendedContentPageRenderer : PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || !(e.NewElement is ExtendedContentPage page))
                return;

            var gradientLayer = new CAGradientLayer
            {
                Frame = View.Bounds,
                Colors = new[] { page.StartColor.ToCGColor(), page.EndColor.ToCGColor() }
            };

            View.Layer.InsertSublayer(gradientLayer, 0);
        }
    }
}