using System;
using System.ComponentModel;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.View;
using Android.Views;
using TestApp.Controls;
using TestApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(RoundedContentView), typeof(RoundedContentViewRenderer))]
namespace TestApp.Droid.Renderers
{
    public class RoundedContentViewRenderer : VisualElementRenderer<RoundedContentView>
    {
        private bool _disposed;

        public RoundedContentViewRenderer(Android.Content.Context context) : base(context)
        { }

        protected override void OnElementChanged(ElementChangedEventArgs<RoundedContentView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null && e.OldElement == null)
            {
                UpdateBackground();
                UpdateElevation();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName ||
                e.PropertyName == RoundedContentView.CornerRadiusProperty.PropertyName ||
                e.PropertyName == RoundedContentView.GradientStartColorProperty.PropertyName ||
                e.PropertyName == RoundedContentView.GradientEndColorProperty.PropertyName ||
                e.PropertyName == RoundedContentView.BorderColorProperty.PropertyName ||
                e.PropertyName == RoundedContentView.BorderThicknessProperty.PropertyName ||
                e.PropertyName == RoundedContentView.BorderIsDashedProperty.PropertyName)
            {
                UpdateBackground();
                return;
            }

            //if (e.PropertyName == RoundedContentView.ElevationProperty.PropertyName)
            {
                UpdateElevation();
                return;
            }
        }

        protected override bool DrawChild(Canvas canvas, global::Android.Views.View child, long drawingTime)
        {
            if (Element == null)
                return false;

            var control = (RoundedContentView)Element;

            SetClipChildren(true);

            //Create path to clip the child.
            using (var path = new Path())
            {
                path.AddRoundRect(new RectF(0, 0, Width, Height), GetRadii(control), Path.Direction.Ccw);

                canvas.Save();
                canvas.ClipPath(path);
            }

            //Draw the child first so that the border shows up above it.        
            var result = base.DrawChild(canvas, child, drawingTime);
            canvas.Restore();

            DrawBorder(canvas, control);

            return result;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing && !_disposed)
            {
                Background.Dispose();
                _disposed = true;
            }
        }

        private void DrawBorder(Canvas canvas, RoundedContentView control)
        {
            var borderThickness = Context.ToPixels(control.BorderThickness);
            var halfBorderThickness = borderThickness / 2f;

            using (var paint = new Paint { AntiAlias = true })
            using (var path = new Path())
            using (var direction = Path.Direction.Cw)
            using (var style = Paint.Style.Stroke)

            using (var rect = new RectF(halfBorderThickness,
                                        halfBorderThickness,
                                        canvas.Width - halfBorderThickness,
                                        canvas.Height - halfBorderThickness))
            {
                path.AddRoundRect(rect, GetRadii(control), direction);

                if (control.BorderIsDashed)
                    paint.SetPathEffect(new DashPathEffect(new[] { 10f, 5f }, 0));
                
                paint.StrokeCap = Paint.Cap.Square;
                paint.StrokeWidth = borderThickness;
                paint.SetStyle(style);
                paint.Color = control.BorderColor.ToAndroid();

                canvas.DrawPath(path, paint);
            }
        }

        private void UpdateBackground()
        {
            this.SetBackground(new RoundedDrawable(Element as RoundedContentView, Context.ToPixels));
        }

        private void UpdateElevation()
        {
            Elevation = (float)10;
            TranslationZ = (float)10;

            //We need to reset the StateListAnimator to override the setting of Elevation on touch down and release.
            StateListAnimator = new Android.Animation.StateListAnimator();

            //Set the elevation manually.
            ViewCompat.SetElevation(this, Elevation);
            ViewCompat.SetElevation(RootView, Elevation);

            //If it has a shadow, give it a default Droid looking shadow.
            if (10 > 0)
            {
                //To have shadow show up, we need to clip. However, clipping means that individual corners are lost :(
                OutlineProvider = new RoundedCornerOutlineProvider(Context.ToPixels(Element.CornerRadius), (int)Context.ToPixels(Element.BorderThickness));
                ClipToOutline = true;
            }
            else
            {
                //To have shadow show up, we need to clip. However, clipping means that individual corners are lost :(
                OutlineProvider = null;
                ClipToOutline = false;
            }
        }

        private float[] GetRadii(RoundedContentView control)
        {
            var topLeft = Context.ToPixels(control.CornerRadius);
            var topRight = Context.ToPixels(control.CornerRadius);
            var bottomRight = Context.ToPixels(control.CornerRadius);
            var bottomLeft = Context.ToPixels(control.CornerRadius);

            var radii = new[] { topLeft, topLeft, topRight, topRight, bottomRight, bottomRight, bottomLeft, bottomLeft };

            if (10 > 0)
                radii = new[] { topLeft, topLeft, topLeft, topLeft, topLeft, topLeft, topLeft, topLeft };

            return radii;
        }
    }

    public class RoundedCornerOutlineProvider : ViewOutlineProvider
    {
        private readonly float _cornerRadius;
        private readonly int _border;

        public RoundedCornerOutlineProvider(float cornerRadius, int border)
        {
            _cornerRadius = cornerRadius;
            _border = border;
        }

        public override void GetOutline(global::Android.Views.View view, Outline outline)
        {
            //TODO: Figure out how to clip individual rounded corners with different radii.
            outline.SetRoundRect(new Rect(0, 0, view.Width, view.Height), _cornerRadius);
        }
    }

    public class RoundedDrawable : Drawable
    {
        private readonly RoundedContentView _element;
        private readonly Func<double, float> _convertToPixels;
        private Bitmap _normalBitmap;
        private bool _isDisposed;

        public override int Opacity => 0;

        public RoundedDrawable(RoundedContentView element, Func<double, float> convertToPixels)
        {
            _element = element;
            _convertToPixels = convertToPixels;
            _element.PropertyChanged += Drawable_PropertyChanged;
        }

        public override void Draw(Canvas canvas)
        {
            var width = Bounds.Width();
            var height = Bounds.Height();

            if (width <= 0 || height <= 0)
            {
                if (_normalBitmap != null)
                {
                    _normalBitmap.Dispose();
                    _normalBitmap = null;
                }
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

        public override void SetColorFilter(ColorFilter colorFilter)
        { }

        protected override bool OnStateChange(int[] state)
        {
            return false;
        }

        private Bitmap CreateBitmap(bool pressed, int width, int height)
        {
            Bitmap bitmap;

            using (var config = Bitmap.Config.Argb8888)
                bitmap = Bitmap.CreateBitmap(width, height, config);

            using (var canvas = new Canvas(bitmap))
                DrawCanvas(canvas, width, height, pressed);

            return bitmap;
        }

        private void DrawBackground(Canvas canvas, int width, int height, CornerRadius cornerRadius, bool pressed)
        {
            using (var paint = new Paint { AntiAlias = true })
            using (var path = new Path())
            using (var direction = Path.Direction.Cw)
            using (var style = Paint.Style.Fill)
            using (var rect = new RectF(0, 0, width, height))
            {
                var topLeft = _convertToPixels(cornerRadius.TopLeft);
                var topRight = _convertToPixels(cornerRadius.TopRight);
                var bottomRight = _convertToPixels(cornerRadius.BottomRight);
                var bottomLeft = _convertToPixels(cornerRadius.BottomLeft);

                if (10 > 0)
                    path.AddRoundRect(rect, new[] { topLeft, topLeft, topLeft, topLeft, topLeft, topLeft, topLeft, topLeft }, direction);
                else
                    path.AddRoundRect(rect, new[] { topLeft, topLeft, topRight, topRight, bottomRight, bottomRight, bottomLeft, bottomLeft }, direction);

                if (_element.GradientStartColor != default && _element.GradientEndColor != default)
                {
                    var angle = 0 / 360.0;

                    //Calculate the new positions based on angle between 0-360.
                    var a = width * Math.Pow(Math.Sin(2 * Math.PI * ((angle + 0.75) / 2)), 2);
                    var b = height * Math.Pow(Math.Sin(2 * Math.PI * ((angle + 0.0) / 2)), 2);
                    var c = width * Math.Pow(Math.Sin(2 * Math.PI * ((angle + 0.25) / 2)), 2);
                    var d = height * Math.Pow(Math.Sin(2 * Math.PI * ((angle + 0.5) / 2)), 2);

                    var shader = new LinearGradient(width - (float)a, (float)b, width - (float)c, (float)d, _element.GradientStartColor.ToAndroid(), _element.GradientEndColor.ToAndroid(), Shader.TileMode.Clamp);
                    paint.SetShader(shader);
                }
                else
                {
                    paint.Color = _element.BackgroundColor.ToAndroid();
                    paint.SetStyle(style);
                }

                canvas.DrawPath(path, paint);
            }
        }

        private void Drawable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName ||
                e.PropertyName == RoundedContentView.CornerRadiusProperty.PropertyName ||
                e.PropertyName == RoundedContentView.GradientStartColorProperty.PropertyName ||
                e.PropertyName == RoundedContentView.GradientEndColorProperty.PropertyName ||
                e.PropertyName == RoundedContentView.BorderColorProperty.PropertyName ||
                e.PropertyName == RoundedContentView.BorderThicknessProperty.PropertyName ||
                e.PropertyName == RoundedContentView.BorderIsDashedProperty.PropertyName)
            {
                if (_normalBitmap == null)
                    return;

                using (var canvas = new Canvas(_normalBitmap))
                {
                    var width = Bounds.Width();
                    var height = Bounds.Height();
                    canvas.DrawColor(Android.Graphics.Color.Black, PorterDuff.Mode.Clear);

                    DrawCanvas(canvas, width, height, false);
                }

                InvalidateSelf();
            }
        }

        private void DrawCanvas(Canvas canvas, int width, int height, bool pressed)
        {
            DrawBackground(canvas, width, height, _element.CornerRadius, pressed);
        }

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
    }
}