using System;

namespace Alloy.HideTabs.VisibilityAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class HidePropertyWhenValueEquals: PropertyVisibilityAttributeBase
    {
        public HidePropertyWhenValueEquals(string propertyName, object value) : base(propertyName, value)
        {
        }
    }
}
