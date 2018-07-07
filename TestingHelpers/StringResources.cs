using System.Reflection;
using System.Resources;

namespace System.IO.Abstractions.TestingHelpers
{
    internal static class StringResources
    {
        public static ResourceManager Manager { get; } = new ResourceManager(
#if NET40
            $"{typeof(StringResources).Namespace}.Properties.Resources", typeof(StringResources).Assembly);
#else
            $"{typeof(StringResources).Namespace}.Properties.Resources", typeof(StringResources).GetTypeInfo().Assembly);
#endif
    }
}
