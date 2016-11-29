using System.Reflection;
using System.Resources;

namespace System.IO.Abstractions.TestingHelpers
{
    internal static class StringResources
    {
        public static ResourceManager Manager { get; } = new ResourceManager("Resources", typeof(StringResources).GetTypeInfo().Assembly);
    }
}
