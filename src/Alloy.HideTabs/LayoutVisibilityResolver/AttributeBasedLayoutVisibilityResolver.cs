using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Alloy.HideTabs.VisibilityAttributes;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;

namespace Alloy.HideTabs.LayoutVisibilityResolver
{
    [ServiceConfiguration(typeof(ILayoutVisibilityResolver))]
    public class AttributeBasedLayoutVisibilityResolver: ILayoutVisibilityResolver
    {
        public IEnumerable<string> GetHiddenTabs(IContent content)
        {
            var result = new List<string>();

            var modelType = content.GetOriginalType();
            foreach (var property in modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attributes = property.GetCustomAttributes();
                foreach (var attribute in attributes.OfType<TabVisibilityAttributeBase>())
                {
                    if (attribute is ShowTabWhenPropertyEqualsAttribute)
                    {
                        var attr = (ShowTabWhenPropertyEqualsAttribute)attribute;
                        if (attr.Value.Equals(content.Property[property.Name].Value) == false)
                        {
                            result.Add(attr.TabName);
                        }
                        continue;
                    }
                    if (attribute is HideTabWhenPropertyEqualsAttribute)
                    {
                        var attr = (HideTabWhenPropertyEqualsAttribute)attribute;
                        if (attr.Value.Equals(content.Property[property.Name].Value) == true)
                        {
                            result.Add(attr.TabName);
                        }
                    }
                }
            }

            return result.GroupBy(x => x).Select(x=>x.FirstOrDefault());
        }

        public IEnumerable<string> GetHiddenProperties(IContent content)
        {
            var result = new List<string>();

            var modelType = content.GetOriginalType();
            foreach (var property in modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attributes = property.GetCustomAttributes();
                foreach (var attribute in attributes.OfType<PropertyVisibilityAttributeBase>())
                {
                    if (attribute is ShowPropertyWhenValueEqualsAttribute)
                    {
                        var attr = (ShowPropertyWhenValueEqualsAttribute)attribute;
                        if (attr.Value.Equals(content.Property[property.Name].Value) == false)
                        {
                            result.Add(attr.PropertyName);
                        }
                        continue;
                    }
                    if (attribute is HidePropertyWhenValueEquals)
                    {
                        var attr = (HidePropertyWhenValueEquals)attribute;
                        if (attr.Value.Equals(content.Property[property.Name].Value) == true)
                        {
                            result.Add(attr.PropertyName);
                        }
                    }
                }
            }

            return result.GroupBy(x => x).Select(x => x.FirstOrDefault());
        }
    }
}
