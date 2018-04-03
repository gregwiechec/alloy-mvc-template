using System;

namespace Alloy.GroupingHeader
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GroupingHeaderAttribute: Attribute
    {
        public string Title { get; set; }
        public string ClassName { get; set; }
        public string Tag { get; set; }

        public GroupingHeaderAttribute(string title = "", string className = "", string tag = "h3")
        {
            this.Title = title;
            this.ClassName = className;
            this.Tag = tag;
        }
    }
}
