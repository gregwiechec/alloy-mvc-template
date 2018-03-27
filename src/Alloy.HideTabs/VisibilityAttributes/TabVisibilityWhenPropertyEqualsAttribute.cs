using System;

namespace Alloy.HideTabs.VisibilityAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ShowTabWhenPropertyEqualsAttribute: TabVisibilityAttributeBase
    {
        public ShowTabWhenPropertyEqualsAttribute(string tabName, object value) : base(tabName, value)
        {
        }
    }
}
