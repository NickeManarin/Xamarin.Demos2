using System;
using System.Linq;
using Android.Widget;
using TestApp.Droid.Effects;
using TestApp.Effects;
using TestApp.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ResolutionGroupName(Constants.AppName)]
[assembly: ExportEffect(typeof(ShadowEffectRenderer), nameof(ShadowEffect))]
namespace TestApp.Droid.Effects
{
    public class ShadowEffectRenderer : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                var control = Control as TextView;
                var effect = (ShadowEffect)Element.Effects.FirstOrDefault(e => e is ShadowEffect);

                if (effect == null || control == null)
                    return;

                control.SetShadowLayer(effect.Radius, effect.DistanceX, effect.DistanceY, effect.Color.ToAndroid());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: " + ex.Message);
            }
        }

        protected override void OnDetached()
        {
            try
            {
                var control = Control as TextView;

                if (control == null)
                    return;

                control.SetShadowLayer(0, 0, 0, Color.Transparent);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot remove property on attached control. Error: " + e.Message);
            }
        }
    }
}