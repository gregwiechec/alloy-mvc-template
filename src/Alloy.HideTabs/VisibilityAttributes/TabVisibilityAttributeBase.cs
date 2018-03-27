using System;

namespace Alloy.HideTabs.VisibilityAttributes
{
    public abstract class TabVisibilityAttributeBase: Attribute, ILayoutElementVisibilityAttribute
    {
        public string TabName { get; set; }

        public object Value { get; set; }

        protected TabVisibilityAttributeBase(string tabName, object value)
        {
            TabName = tabName;
            Value = value;
        }
    }
}
