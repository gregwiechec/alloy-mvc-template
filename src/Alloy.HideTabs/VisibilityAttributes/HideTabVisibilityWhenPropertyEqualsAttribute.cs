using System;

namespace Alloy.HideTabs.VisibilityAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class HideTabWhenPropertyEqualsAttribute : TabVisibilityAttributeBase
    {
        public HideTabWhenPropertyEqualsAttribute(string tabName, object value) : base(tabName, value)
        {
        }
    }
}
