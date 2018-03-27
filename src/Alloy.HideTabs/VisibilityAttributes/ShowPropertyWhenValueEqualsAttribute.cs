using System;

namespace Alloy.HideTabs.VisibilityAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ShowPropertyWhenValueEqualsAttribute: PropertyVisibilityAttributeBase
    {
        public ShowPropertyWhenValueEqualsAttribute(string propertyName, object value) : base(propertyName, value)
        {
        }
    }
}
