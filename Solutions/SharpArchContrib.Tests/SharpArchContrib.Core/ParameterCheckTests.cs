namespace Tests.SharpArchContrib.Core
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using SharpArch.Core;

    using global::SharpArchContrib.Core;

    [TestFixture]
    public class ParameterCheckTests
    {
        [Test]
        public void DictionaryContainKey_Supports_Dictionary_With_String_Key_And_Value()
        {
            var dict = new Dictionary<string, string>();
            Assert.Throws<PreconditionException>(() => ParameterCheck.DictionaryContainsKey(dict, "dict", "key"));
        }
    }
}