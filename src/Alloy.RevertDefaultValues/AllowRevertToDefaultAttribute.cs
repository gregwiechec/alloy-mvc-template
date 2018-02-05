using System;

namespace Alloy.RevertDefaultValues
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AllowRevertToDefaultAttribute: Attribute
    {
        public bool AllowRevert { get; set; }

        public AllowRevertToDefaultAttribute(bool allowRevert = true)
        {
            this.AllowRevert = allowRevert;
        }
    }
}
