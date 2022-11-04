#if !FEATURE_SUPPORTED_OS_ATTRIBUTE
namespace System.Runtime.Versioning
{
    [AttributeUsage(AttributeTargets.All)]
    internal class SupportedOSPlatformAttribute : Attribute
    {
        public SupportedOSPlatformAttribute(string _)
        {
        }
    }
}
#endif
