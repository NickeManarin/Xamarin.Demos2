using System;
using System.ComponentModel;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using ACanvas = Android.Graphics.Canvas;
using TestApp.Controls;
using TestApp.Droid.Renderers;
using TestApp.Controls;
using TestApp.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedFrame), typeof(ExtendedFrameRenderer))]
namespace TestApp.Droid.Renderers
{
    public class ExtendedFrameRenderer : FrameRenderer
    {
        public ExtendedFrameRenderer(Context context) : base(context)
        { }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null && e.OldElement == null)
                this.SetBackground(new FrameDrawable(Element as ExtendedFrame, Context.ToPixels));
        }

        internal class FrameDrawable : Drawable
        {
            readonly ExtendedFrame _frame;
            readonly Func<double, float> _convertToPixels;

            bool _isDisposed;
            Bitmap _normalBitmap;

            public FrameDrawable(ExtendedFrame frame, Func<double, float> convertToPixels)
            {
                _frame = frame;
                _convertToPixels = convertToPixels;
                frame.PropertyChanged += FrameOnPropertyChanged;
            }

            public override bool IsStateful => false;

            public override int Opacity => 0;

            public override void Draw(ACanvas canvas)
            {
                var width = Bounds.Width();
                var height = Bounds.Height();

                if (width <= 0 || height <= 0)
                {
                    if (_normalBitmap == null)
                        return;

                    _normalBitmap.Dispose();
                    _normalBitmap = null;

                    return;
                }

                if (_normalBitmap == null || _normalBitmap.Height != height || _normalBitmap.Width != width)
                {
                    //If the user changes the orientation of the screen, make sure to destroy reference before reassigning a new bitmap reference.
                    if (_normalBitmap != null)
                    {
                        _normalBitmap.Dispose();
                        _normalBitmap = null;
                    }

                    _normalBitmap = CreateBitmap(false, width, height);
                }

                var bitmap = _normalBitmap;

                using (var paint = new Paint())
                    canvas.DrawBitmap(bitmap, 0, 0, paint);
            }

            public override void SetAlpha(int alpha)
            { }

            public override void SetColorFilter(ColorFilter cf)
            { }

            protected override void Dispose(bool disposing)
            {
                if (disposing && !_isDisposed)
                {
                    if (_normalBitmap != null)
                    {
                        _normalBitmap.Dispose();
                        _normalBitmap = null;
                    }

                    _isDisposed = true;
                }

                base.Dispose(disposing);
            }

            protected override bool OnStateChange(int[] state)
            {
                return false;
            }

            private Bitmap CreateBitmap(bool pressed, int width, int height)
            {
                Bitmap bitmap;

                using (var config = Bitmap.Config.Argb8888)
                    bitmap = Bitmap.CreateBitmap(width, height, config);

                using (var canvas = new ACanvas(bitmap))
                    DrawCanvas(canvas, width, height, pressed);

                return bitmap;
            }

            private void DrawBackground(ACanvas canvas, Path path, bool pressed)
            {
                using (var paint = new Paint { AntiAlias = true })
                using (var style = Paint.Style.Fill)
                {
                    paint.SetStyle(style);
                    paint.Color = _frame.BackgroundColor.ToAndroid();

                    canvas.DrawPath(path, paint);
                }
            }

            private void DrawOutline(ACanvas canvas, Path path)
            {
                using (var paint = new Paint { AntiAlias = true })
                using (var style = Paint.Style.Stroke)
                {
                    paint.StrokeWidth = (float)_frame.StrokeThickness;
                    paint.SetStyle(style);
                    paint.Color = _frame.BorderColor.ToAndroid();

                    if (_frame.StrokeDashLength > 0 && _frame.StrokeDashGap > 0)
                        paint.SetPathEffect(new DashPathEffect(new[] { (float)_frame.StrokeDashLength, (float)_frame.StrokeDashGap }, 0));

                    canvas.DrawPath(path, paint);
                }
            }

            private void FrameOnPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName ||
                    e.PropertyName == Frame.BorderColorProperty.PropertyName ||
                    e.PropertyName == Frame.CornerRadiusProperty.PropertyName ||
                    e.PropertyName == ExtendedFrame.StrokeThicknessProperty.PropertyName ||
                    e.PropertyName == ExtendedFrame.StrokeDashLengthProperty.PropertyName ||
                    e.PropertyName == ExtendedFrame.StrokeDashGapProperty.PropertyName ||
                    e.PropertyName == ExtendedFrame.CornersProperty.PropertyName)
                {
                    if (_normalBitmap == null)
                        return;

                    using (var canvas = new ACanvas(_normalBitmap))
                    {
                        var width = Bounds.Width();
                        var height = Bounds.Height();

                        canvas.DrawColor(global::Android.Graphics.Color.Black, PorterDuff.Mode.Clear);
                        DrawCanvas(canvas, width, height, false);
                    }

                    InvalidateSelf();
                }
            }

            private void DrawCanvas(ACanvas canvas, int width, int height, bool pressed)
            {
                var cornerRadius = _frame.CornerRadius;

                if (cornerRadius < 0)
                    cornerRadius = 5f; //Default corner radius.

                var left = 0f + (float)_frame.StrokeThickness;
                var top = 0f + (float)_frame.StrokeThickness;
                var right = left + width - ((float)_frame.StrokeThickness * 2f);
                var bottom = top + height - ((float)_frame.StrokeThickness * 2f);
                var radius = _convertToPixels(cornerRadius);

                using (var path = new Path())
                {
                    switch (_frame.Corners)
                    {
                        case Enums.RoundedCorners.None:
                            path.MoveTo(left, top);
                            path.LineTo(right, top);
                            path.LineTo(right, bottom);
                            path.LineTo(left, bottom);
                            //path.LineTo(left, bottom);
                            path.LineTo(left, top);
                            break;
                        case Enums.RoundedCorners.Left:
                            path.MoveTo(right, top); //Starts at the top right corner.
                            path.LineTo(left + radius, top); //Top, goes to left.
                            path.QuadTo(left, top, left, top + radius); //Draws top-left rounded corner.
                            path.LineTo(left, bottom - radius); //Draws line to the bottom-left corner.
                            path.QuadTo(left, bottom, left + radius, bottom);
                            path.LineTo(right, bottom);
                            path.LineTo(right, top);
                            break;
                        case Enums.RoundedCorners.Right:
                            path.MoveTo(left, top); //Starts at the top left corner.
                            path.LineTo(right - radius, top); //Top, goes to right.
                            path.QuadTo(right, top, right, top + radius); //Top right curve.
                            path.LineTo(right, bottom - radius); //Right.
                            path.QuadTo(right, bottom, right - radius, bottom); //Bottom right curve.
                            path.LineTo(left, bottom); //Bottom.
                            path.LineTo(left, top); //Left.
                            break;
                        case Enums.RoundedCorners.Top: //TODO:

                            break;
                        case Enums.RoundedCorners.Bottom: //TODO:

                            break;
                        case Enums.RoundedCorners.Vertical: //TODO:

                            break;
                        case Enums.RoundedCorners.Horizontal: //TODO:

                            break;
                        case Enums.RoundedCorners.All:
                            path.MoveTo(left + radius, top);
                            path.LineTo(right - radius, top);
                            path.QuadTo(right, top, right, top + radius);
                            path.LineTo(right, bottom - radius);
                            path.QuadTo(right, bottom, right - radius, bottom);
                            path.LineTo(left + radius, bottom);
                            path.QuadTo(left, bottom, left, bottom - radius);
                            path.LineTo(left, top + radius);
                            path.QuadTo(left, top, left + radius, top);
                            break;
                    }

                    path.Close();

                    DrawBackground(canvas, path, pressed);
                    DrawOutline(canvas, path);
                }
            }
        }
    }
}