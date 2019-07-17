using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TestApp.Helpers;
using Xamarin.Forms;

namespace TestApp.Controls
{
    public class GradientContentPage : ContentPage
    {
        public List<Color> ColorsList { get; set; }

        public string ColorsString { get; set; }

        public Enums.GradientDirection GradientDirection { get; set; }

        public List<Color> GetColors()
        {
            if (ColorsList != null)
                return ColorsList;

            var hex = ColorsString.Split(',');

            return hex.Select(t => Color.FromHex(t.Trim())).ToList();
        }

        /// <summary>
        /// Rotates (360°) the given element.
        /// </summary>
        /// <param name="element">The element to be rotated.</param>
        /// <param name="duration">The duration in miliseconds.</param>
        /// <param name="cancellation">The cancelation token.</param>
        /// <returns></returns>
        protected async Task RotateElement(VisualElement element, uint duration, CancellationToken cancellation)
        {
            while (!cancellation.IsCancellationRequested)
            {
                await element.RotateTo(360, duration, Easing.Linear);
                await element.RotateTo(0, 0); //Resets to the initial position.
            }
        }
    }
}