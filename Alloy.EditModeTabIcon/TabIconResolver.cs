using System.Collections.Concurrent;
using EPiServer.ServiceLocation;

namespace Alloy.EditModeTabIcon
{
    [ServiceConfiguration(typeof(TabIconResolver), Lifecycle = ServiceInstanceScope.Singleton)]
    public class TabIconResolver
    {
        private readonly ConcurrentDictionary<string, string> _icons = new ConcurrentDictionary<string, string>();
        private readonly ConcurrentDictionary<string, bool> _titlesVisibility = new ConcurrentDictionary<string, bool>();

        public TabIconResolver()
        {
            this.SetIcon("Information", "epi-iconPen");
            this.SetIcon("Advanced", "epi-iconSettings");
        }

        public virtual string GetIcon(string groupName)
        {
            string result;
            if (_icons.TryGetValue(groupName, out result))
            {
                return result;
            }
            return string.Empty;
        }

        public virtual void SetIcon(string groupName, string iconClass)
        {
            this._icons.AddOrUpdate(groupName, iconClass, (key, value) => iconClass);
        }

        public virtual bool IsTitleVisible(string groupName)
        {
            bool result;
            if (_titlesVisibility.TryGetValue(groupName, out result))
            {
                return result;
            }
            return true;
        }

        public virtual void SetTitleVisible(string groupName, bool visible)
        {
            this._titlesVisibility.AddOrUpdate(groupName, visible, (key, value) => visible);
        }
    }
}
