using System;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    internal sealed class SkipOnUnixAttribute : Attribute, ITestAction
    {
        private readonly string reason;

        public SkipOnUnixAttribute(string reason)
        {
            this.reason = reason;
        }

        public ActionTargets Targets => ActionTargets.Test;

        public void BeforeTest(ITest test)
        {
            if (MockUnixSupport.IsUnixPlatform())
            {
                Assert.Inconclusive(this.reason);
            }
        }

        public void AfterTest(ITest test) { }
    }
}
