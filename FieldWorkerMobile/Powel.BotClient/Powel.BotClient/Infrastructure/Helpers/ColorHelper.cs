using System.Collections.Generic;
using Xamarin.Forms;

namespace Powel.BotClient.Infrastructure.Helpers
{
    public static class ColorHelper
    {
        public static Color GetColor(string color)
        {
            if (!Application.Current.Resources.ContainsKey(color))
            {
                throw new KeyNotFoundException("There is no color in the resources that matches " + color);
            }

            return (Color)Application.Current.Resources[color];
        }
    }

}