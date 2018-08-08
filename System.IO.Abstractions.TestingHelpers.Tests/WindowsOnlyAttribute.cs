using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    internal sealed class WindowsOnlyAttribute : Attribute, ITestAction
    {
        private readonly string reason;

        public WindowsOnlyAttribute(string reason)
        {
            this.reason = reason;
        }

        public ActionTargets Targets => ActionTargets.Test;

        public void BeforeTest(ITest test)
        {
            if (MockUnixSupport.IsUnixPlatform())
            {
                Assert.Inconclusive(reason);
            }
        }

        public void AfterTest(ITest test) { }
    }
}
