using AMapAPIforWP8Demo.Resources;

namespace AMapAPIforWP8Demo
{
    /// <summary>
    /// 提供对字符串资源的访问权。
    /// </summary>
    public class LocalizedStrings
    {
        private static AppResources _localizedResources = new AppResources();

        public AppResources LocalizedResources { get { return _localizedResources; } }

        public static Resource1 resource = new Resource1();
        public Resource1 Resource1 { get { return resource; } }
    }
}