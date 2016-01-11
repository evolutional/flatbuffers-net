namespace FlatBuffers.Utilities
{
    /// <summary>
    /// A collection of utilities for strings
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// Converts the given string to camel case
        /// </summary>
        /// <param name="str">String to convert</param>
        /// <returns>Camel Cased string</returns>
        public static string ToCamelCase(this string str)
        {
            if (str.Length <= 2)
            {
                return str;
            }
            var chars = str.ToCharArray();

            chars[0] = char.ToLower(chars[0]);
            return new string(chars);
        }
    }
}
