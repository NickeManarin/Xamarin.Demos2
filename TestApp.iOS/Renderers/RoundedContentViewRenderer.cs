using System.ComponentModel;
using CoreAnimation;
using TestApp.Controls;
using TestApp.iOS.Renderers;
using TestApp.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RoundedContentView), typeof(RoundedContentViewRenderer))]
namespace TestApp.iOS.Renderers
{
    public class RoundedContentViewRenderer : VisualElementRenderer<ContentView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ContentView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
                SetupLayer();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName ||
                e.PropertyName == RoundedContentView.BorderColorProperty.PropertyName ||
                e.PropertyName == RoundedContentView.CornerRadiusProperty.PropertyName ||
                e.PropertyName == RoundedContentView.GradientStartColorProperty.PropertyName ||
                e.PropertyName == RoundedContentView.GradientEndColorProperty.PropertyName ||
                e.PropertyName == RoundedContentView.HasBackgroundGradientProperty.PropertyName)
            {
                SetupLayer();
                SetNeedsDisplay();
            }
        }

        private void SetupLayer()
        {
            var rcv = Element as RoundedContentView;
            var cornerRadius = rcv.CornerRadius;

            if (cornerRadius == -1f)
                cornerRadius = 5f; //Default corner radius.

            Layer.CornerRadius = cornerRadius;

            if (rcv.BorderColor == Color.Default)
                Layer.BorderColor = UIColor.Clear.CGColor;
            else
            {
                Layer.BorderColor = rcv.BorderColor.ToCGColor();
                Layer.BorderWidth = 1;
            }

            Layer.RasterizationScale = UIScreen.MainScreen.Scale;
            Layer.ShouldRasterize = true;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var rcv = Element as RoundedContentView;

            if (rcv.HasBackgroundGradient) //Perform initial setup.
            {
                var gradientLayer = new CAGradientLayer
                {
                    Frame = NativeView.Bounds,
                    Colors = new[] { rcv.GradientStartColor.ToCGColor(), rcv.GradientEndColor.ToCGColor() }
                };

                if (NativeView.Layer.Sublayers != null && NativeView.Layer.Sublayers.Length > 0 && NativeView.Layer.Sublayers[0].GetType() == typeof(CAGradientLayer))
                    NativeView.Layer.ReplaceSublayer(NativeView.Layer.Sublayers[0], gradientLayer);
                else
                    NativeView.Layer.InsertSublayer(gradientLayer, 0);
            }
            else
            {
                Layer.BackgroundColor = Element.BackgroundColor == Color.Default ? UIColor.Clear.CGColor : Element.BackgroundColor.ToCGColor();
            }
        }
    }
}