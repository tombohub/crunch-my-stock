namespace Crunch.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Capitalize first letter in a string
        /// </summary>
        /// <param name="str">string to capitalize</param>
        /// <returns>Capitalized string</returns>
        public static string Capitalize(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            return char.ToUpper(str[0]) + str.Substring(1);
        }
    }
}