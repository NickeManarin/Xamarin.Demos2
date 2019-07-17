using System;
using System.Linq;
using CoreGraphics;
using TestApp.iOS.Effects;
using TestApp.Effects;
using TestApp.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName(Constants.AppName)]
[assembly: ExportEffect(typeof(ShadowEffectRenderer), nameof(ShadowEffect))]
namespace TestApp.iOS.Effects
{
    public class ShadowEffectRenderer : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                var effect = (ShadowEffect)Element.Effects.FirstOrDefault(e => e is ShadowEffect);

                if (effect == null)
                    return;

                Control.Layer.CornerRadius = effect.Radius;
                Control.Layer.ShadowColor = effect.Color.ToCGColor();
                Control.Layer.ShadowOffset = new CGSize(effect.DistanceX, effect.DistanceY);
                Control.Layer.ShadowOpacity = 1.0f;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: " + ex.Message);
            }
        }

        protected override void OnDetached()
        {
            Control.Layer.ShadowOpacity = 0f;
        }
    }
}