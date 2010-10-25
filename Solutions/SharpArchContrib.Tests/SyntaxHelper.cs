namespace Tests
{
    using System;

    using NUnit.Framework;

    public static class SyntaxHelper
    {
        public static void ShouldEqualInMemorySqlDateTime(this DateTime actual, DateTime expected)
        {
            var expectedWithoutMilliseconds = actual.AddMilliseconds(-1 * actual.Millisecond);
            Assert.AreEqual(actual, expectedWithoutMilliseconds);
        }

        public static void ShouldNotContain(this string actual, string expected)
        {
            StringAssert.DoesNotContain(expected, actual);
        }
    }
}