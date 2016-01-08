using System.Text;
using NUnit.Framework;

namespace FlatBuffers.Tests
{
    public static class AssertExtensions
    {
        private static string NormalizeWhitespace(string value)
        {
            var lastChar = '\0';
            var sb = new StringBuilder();
            for (var i = 0; i < value.Length; ++i)
            {
                var c = value[i];
                if (c == '\n' || c == '\r')
                {
                    c = ' ';
                }

                if (char.IsWhiteSpace(lastChar) && char.IsWhiteSpace(c))
                {
                    lastChar = c;
                    continue;
                }
                
                sb.Append(c);
                lastChar = c;
            }
            return sb.ToString().Trim();
        }

        public static void AreEquivalent(string expected, string actual)
        {
            expected = NormalizeWhitespace(expected);
            actual = NormalizeWhitespace(actual);
            Assert.AreEqual(expected, actual);
        }
    }
}