using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestApp.Helpers
{
    internal static class VisualHelper
    {
        public static Task<bool> BackgroundColorTo(this VisualElement self, Color fromColor, Color toColor, uint length = 250, Easing easing = null)
        {
            Color Transform(double t) => Color.FromRgba(fromColor.R + t * (toColor.R - fromColor.R), fromColor.G + t * (toColor.G - fromColor.G),
                fromColor.B + t * (toColor.B - fromColor.B), fromColor.A + t * (toColor.A - fromColor.A));

            return ColorAnimation(self, "BackgroundColorTo", Transform, a => self.BackgroundColor = a, length, easing);
        }

        public static Task<bool> ColorTo(this VisualElement self, Color fromColor, Color toColor, Action<Color> callback, uint length = 250, Easing easing = null)
        {
            Color Transform(double t) => Color.FromRgba(fromColor.R + t * (toColor.R - fromColor.R), fromColor.G + t * (toColor.G - fromColor.G),
                fromColor.B + t * (toColor.B - fromColor.B), fromColor.A + t * (toColor.A - fromColor.A));

            return ColorAnimation(self, "ColorTo", Transform, callback, length, easing);
        }

        public static Task<bool> HeightRequestTo(this VisualElement self, double fromValue, double toValue, uint length = 250, Easing easing = null)
        {
            return DoubleAnimation(self, "HeightRequestTo", t => fromValue + (toValue - fromValue) * t, a => self.HeightRequest = a, length, easing);
        }



        public static void CancelBackgroundColorAnimation(this VisualElement self)
        {
            self.AbortAnimation("BackgroundColorTo");
        }

        public static void CancelColorAnimation(this VisualElement self)
        {
            self.AbortAnimation("ColorTo");
        }


        private static Task<bool> ColorAnimation(VisualElement element, string name, Func<double, Color> transform, Action<Color> callback, uint length, Easing easing)
        {
            easing = easing ?? Easing.Linear;
            var taskCompletionSource = new TaskCompletionSource<bool>();

            element.Animate<Color>(name, transform, callback, 16, length, easing, (v, c) => taskCompletionSource.SetResult(c));

            return taskCompletionSource.Task;
        }

        private static Task<bool> DoubleAnimation(VisualElement element, string name, Func<double, double> transform, Action<double> callback, uint length, Easing easing)
        {
            easing = easing ?? Easing.Linear;
            var taskCompletionSource = new TaskCompletionSource<bool>();

            element.Animate<double>(name, transform, callback, 16, length, easing, (v, c) => taskCompletionSource.SetResult(c));

            return taskCompletionSource.Task;
        }

        private static async Task ShakeHorizontally(VisualElement element)
        {
            const uint timeout = 50;
            await element.TranslateTo(-15, 0, timeout);
            await element.TranslateTo(15, 0, timeout);
            await element.TranslateTo(-10, 0, timeout);
            await element.TranslateTo(10, 0, timeout);
            await element.TranslateTo(-5, 0, timeout);
            await element.TranslateTo(5, 0, timeout);
            element.TranslationX = 0;
        }
    }
}