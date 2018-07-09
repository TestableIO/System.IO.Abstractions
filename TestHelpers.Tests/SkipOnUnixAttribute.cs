using System;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    internal static class SkipReason
    {
        public const string NoDrivesOnUnix = "Unix does not have the concept of Drives";
        public const string NoACLsOnUnix = "Unix does not support ACLs";
        public const string NoUNCPathsOnUnix = "Unix does not have the concept of UNC paths";
    }

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
