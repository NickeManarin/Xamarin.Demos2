using System.ComponentModel;
using Android.Content;
using TestApp.Controls;
using TestApp.Droid.Renderers;
using TestApp.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedContentPage), typeof(ExtendedContentPageRenderer))]
namespace TestApp.Droid.Renderers
{
    public class ExtendedContentPageRenderer : PageRenderer
    {
        private Color StartColor { get; set; }
        private Color EndColor { get; set; }

        public ExtendedContentPageRenderer(Context context) : base(context)
        { }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            Android.Util.Log.WriteLine(Android.Util.LogPriority.Debug, "MyApp", e.PropertyName);

            if (e.PropertyName == ExtendedContentPage.StartColorProperty.PropertyName ||
                e.PropertyName == ExtendedContentPage.EndColorProperty.PropertyName ||
                e.PropertyName == "Renderer")
            {
                var page = sender as ExtendedContentPage;

                StartColor = page.StartColor;
                EndColor = page.EndColor;

                Invalidate();
                RefreshDrawableState();
            }
        }

        protected override void DispatchDraw(global::Android.Graphics.Canvas canvas)
        {
            var gradient = new Android.Graphics.LinearGradient(0, 0, Width, 0,
                StartColor.ToAndroid(), EndColor.ToAndroid(),
                Android.Graphics.Shader.TileMode.Mirror);

            var paint = new Android.Graphics.Paint
            {
                Dither = true,
            };
            paint.SetShader(gradient);
            canvas.DrawPaint(paint);

            base.DispatchDraw(canvas);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
                return;

            if (!(e.NewElement is ExtendedContentPage page))
                return;

            StartColor = page.StartColor;
            EndColor = page.EndColor;
        }
    }
}