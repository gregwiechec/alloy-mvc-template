using System;
using System.Collections.Generic;
using System.Linq;
using Alloy.HideTabs.LayoutVisibilityResolver;
using Alloy.HideTabs.VisibilityAttributes;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;

namespace Alloy.HideTabs
{
    [ServiceConfiguration(IncludeServiceAccessor = false)]
    public class HiddenPropertiesMetadataExtender : IMetadataExtender
    {
        private readonly ILayoutVisibilityResolver _layoutVisibilityResolver;

        public HiddenPropertiesMetadataExtender(CompositeLayoutResolver layoutVisibilityResolver)
        {
            _layoutVisibilityResolver = layoutVisibilityResolver;
        }

        public void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            var content = metadata.Model as IContent;
            if (content == null)
            {
                return;
            }
            metadata.CustomEditorSettings["hiddenTabs"] = _layoutVisibilityResolver.GetHiddenTabs(content);
            metadata.CustomEditorSettings["hiddenProperties"] = _layoutVisibilityResolver.GetHiddenProperties(content);

            foreach (var metadataProperty in metadata.Properties.Cast<ExtendedMetadata>())
            {
                if (metadataProperty.Attributes.OfType<RequiresLayoutRefreshAttribute>().Any() ||
                    metadataProperty.Attributes.OfType<ILayoutElementVisibilityAttribute>().Any())
                {
                    metadataProperty.CustomEditorSettings["requiresLayoutRefresh"] = true;
                }
            }
        }
    }
}
