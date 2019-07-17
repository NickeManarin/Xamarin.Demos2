using CoreAnimation;
using CoreGraphics;
using System.Drawing;
using Foundation;
using TestApp.iOS.Renderers;
using TestApp.Controls;
using TestApp.Helpers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedFrame), typeof(ExtendedFrameRenderer))]
namespace TestApp.iOS.Renderers
{
    public class ExtendedFrameRenderer : VisualElementRenderer<ExtendedFrame>
    {
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName ||
                e.PropertyName == Xamarin.Forms.Frame.BorderColorProperty.PropertyName ||
                e.PropertyName == Xamarin.Forms.Frame.CornerRadiusProperty.PropertyName ||
                e.PropertyName == ExtendedFrame.StrokeThicknessProperty.PropertyName ||
                e.PropertyName == ExtendedFrame.StrokeDashLengthProperty.PropertyName ||
                e.PropertyName == ExtendedFrame.StrokeDashGapProperty.PropertyName ||
                e.PropertyName == ExtendedFrame.CornersProperty.PropertyName)
            {
                SetNeedsDisplay();
            }
        }

        public override void Draw(CGRect rect)
        {
            var radius = new SizeF(Element.CornerRadius, Element.CornerRadius);

            CGRect r;
            UIBezierPath path;

            switch (Element.Corners)
            {
                case Enums.RoundedCorners.Left:
                    r = new CGRect(rect.X + 4, rect.Y + 2, rect.Width - 4, rect.Height - 4);
                    path = UIBezierPath.FromRoundedRect(r, UIRectCorner.TopLeft | UIRectCorner.BottomLeft, radius);
                    break;
                case Enums.RoundedCorners.Right:
                    r = new CGRect(rect.X, rect.Y + 2, rect.Width - 4, rect.Height - 4);
                    path = UIBezierPath.FromRoundedRect(r, UIRectCorner.TopRight | UIRectCorner.BottomRight, radius);
                    break;
                //case RoundedCorners.Top:

                //    break;
                //case RoundedCorners.Bottom:

                //    break;
                //case RoundedCorners.Vertical:

                //    break;
                //case RoundedCorners.Horizontal:

                //    break;
                case Enums.RoundedCorners.All:
                    r = new CGRect(rect.X + 2, rect.Y + 2, rect.Width - 4, rect.Height - 4);
                    path = UIBezierPath.FromRoundedRect(r, radius.Width);
                    break;
                case Enums.RoundedCorners.None:
                    r = new CGRect(rect.X, rect.Y + 2, rect.Width, rect.Height - 4);
                    path = UIBezierPath.FromRoundedRect(r, (float)0.0);
                    break;
                default:
                    r = new CGRect(rect.X + 2, rect.Y + 2, rect.Width, rect.Height - 4);
                    path = UIBezierPath.FromRoundedRect(r, radius.Width);
                    break;
            }

            //if (Element == null)
            //{
            //    UIColor.FromRGB(245, 249, 252).SetFill();
            //    path.Fill();
            //    UIColor.FromRGB(186, 198, 210).SetStroke();
            //    path.Stroke();
            //}
            //else
            //{
            //    Element.BackgroundColor.ToUIColor().SetFill();
            //    path.Fill();
            //    Element.BorderColor.ToUIColor().SetStroke();
            //    path.Stroke();
            //}

            var layer = new CAShapeLayer
            {
                StrokeColor = Element.BorderColor.ToCGColor(),
                FillColor = Element.BackgroundColor.ToCGColor(),
                Frame = NativeView.Bounds,
                Path = path.CGPath
            };

            if (Element.StrokeDashLength > 0 && Element.StrokeDashGap > 0)
                layer.LineDashPattern = new[] { new NSNumber(Element.StrokeDashLength), new NSNumber(Element.StrokeDashGap) };

            Layer.AddSublayer(layer);
        }
    }
}