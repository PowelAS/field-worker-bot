using System;
using System.Linq;

namespace Powel.BotClient.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string CapitalizeFirstLetter(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            return input.First().ToString().ToUpper() + string.Join("", input.Skip(1));
        }
    }
}