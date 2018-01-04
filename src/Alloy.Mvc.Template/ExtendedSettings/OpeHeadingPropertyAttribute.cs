using System;

namespace AlloyTemplates.ExtendedSettings
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OpeHeadingPropertyAttribute : Attribute
    {
        public HeaderColumn HeaderColumn { get; set; }

        public OpeHeadingPropertyAttribute(HeaderColumn headerColumn)
        {
            HeaderColumn = headerColumn;
        }

        public OpeHeadingPropertyAttribute() : this(HeaderColumn.Third)
        {
        }
    }
}
