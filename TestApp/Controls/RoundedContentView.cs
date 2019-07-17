using Xamarin.Forms;

namespace TestApp.Controls
{
    public class RoundedContentView : ContentView
    {
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(RoundedContentView), default(float));
        public static readonly BindableProperty BorderThicknessProperty = BindableProperty.Create(nameof(BorderThickness), typeof(int), typeof(RoundedContentView), default(int));
        public static readonly BindableProperty BorderIsDashedProperty = BindableProperty.Create(nameof(BorderIsDashed), typeof(bool), typeof(RoundedContentView), default(bool));
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(RoundedContentView), default(Color));
        public static readonly BindableProperty HasBackgroundGradientProperty = BindableProperty.Create(nameof(HasBackgroundGradient), typeof(bool), typeof(RoundedContentView), default(bool));
        public static readonly BindableProperty GradientStartColorProperty = BindableProperty.Create(nameof(GradientStartColor), typeof(Color), typeof(RoundedContentView), default(Color));
        public static readonly BindableProperty GradientEndColorProperty = BindableProperty.Create(nameof(GradientEndColor), typeof(Color), typeof(RoundedContentView), default(Color));

        public bool HasBackgroundGradient
        {
            get => (bool)GetValue(HasBackgroundGradientProperty);
            set => SetValue(HasBackgroundGradientProperty, value);
        }

        public Color GradientStartColor
        {
            get => (Color)GetValue(GradientStartColorProperty);
            set => SetValue(GradientStartColorProperty, value);
        }

        public Color GradientEndColor
        {
            get => (Color)GetValue(GradientEndColorProperty);
            set => SetValue(GradientEndColorProperty, value);
        }

        public float CornerRadius
        {
            get => (float)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public int BorderThickness
        {
            get => (int)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        public bool BorderIsDashed
        {
            get => (bool)GetValue(BorderIsDashedProperty);
            set => SetValue(BorderIsDashedProperty, value);
        }

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }
    }
}