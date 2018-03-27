using System;

namespace Alloy.HideTabs.VisibilityAttributes
{
    public abstract class PropertyVisibilityAttributeBase: Attribute, ILayoutElementVisibilityAttribute
    {
        public string PropertyName { get; set; }

        public object Value { get; set; }

        protected PropertyVisibilityAttributeBase(string propertyName, object value)
        {
            PropertyName = propertyName;
            Value = value;
        }
    }
}
