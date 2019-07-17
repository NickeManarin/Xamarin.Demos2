using TestApp.Helpers;
using Xamarin.Forms;

namespace TestApp.Effects
{
    public class ShadowEffect : RoutingEffect
    {
        /// <summary>
        /// From 0 to 1. With 0 being fully transparent.
        /// </summary>
        public float Opacity { get; set; }

        public float Radius { get; set; }

        public Color Color { get; set; }

        public float DistanceX { get; set; }

        public float DistanceY { get; set; }

        public ShadowEffect() : base($"{Constants.AppName}.{nameof(ShadowEffect)}")
        { }
    }
}