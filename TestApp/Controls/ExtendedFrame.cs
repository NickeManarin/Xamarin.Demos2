using TestApp.Helpers;
using Xamarin.Forms;

namespace TestApp.Controls
{
    public class ExtendedFrame : Frame
    {
        public static readonly BindableProperty ElevationProperty = BindableProperty.Create(nameof(Elevation), typeof(double), typeof(ExtendedFrame), 0d);

        public static readonly BindableProperty StrokeThicknessProperty = BindableProperty.Create(nameof(StrokeThickness), typeof(double), typeof(ExtendedFrame), 1d);

        public static readonly BindableProperty StrokeDashLengthProperty = BindableProperty.Create(nameof(StrokeDashLength), typeof(double), typeof(ExtendedFrame), double.NaN);

        public static readonly BindableProperty StrokeDashGapProperty = BindableProperty.Create(nameof(StrokeDashGap), typeof(double), typeof(ExtendedFrame), 0d);

        public static readonly BindableProperty CornersProperty = BindableProperty.Create(nameof(Corners), typeof(Enums.RoundedCorners), typeof(ExtendedFrame), Enums.RoundedCorners.All);

        /// <summary>
        /// Decides how thick the border of the frame is.
        /// </summary>
        public double Elevation
        {
            get => (double)GetValue(ElevationProperty);
            set => SetValue(ElevationProperty, value);
        }

        /// <summary>
        /// Decides how thick the border of the frame is.
        /// </summary>
        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        /// <summary>
        /// The length of the stroke dash.
        /// </summary>
        public double StrokeDashLength
        {
            get => (double)GetValue(StrokeDashLengthProperty);
            set => SetValue(StrokeDashLengthProperty, value);
        }

        /// <summary>
        /// The length of the dash gap.
        /// </summary>
        public double StrokeDashGap
        {
            get => (double)GetValue(StrokeDashGapProperty);
            set => SetValue(StrokeDashGapProperty, value);
        }

        /// <summary>
        /// Dicides which corner has the rounding enabled.
        /// </summary>
        public Enums.RoundedCorners Corners
        {
            get => (Enums.RoundedCorners)GetValue(CornersProperty);
            set => SetValue(CornersProperty, value);
        }

        public ExtendedFrame()
        {
            BackgroundColor = Color.Transparent;
            HasShadow = false;
            Padding = new Thickness(0);
        }
    }
}