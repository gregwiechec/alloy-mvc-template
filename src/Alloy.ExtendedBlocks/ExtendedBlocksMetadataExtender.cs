using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Cms.Shell.UI.ObjectEditing;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;

namespace Alloy.ExtendedBlocks
{
    [ServiceConfiguration(IncludeServiceAccessor = false)]
    public class ExtendedBlocksMetadataExtender : IMetadataExtender
    {
       
        public void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            foreach (var property in metadata.Properties.OfType<ContentDataMetadata>())
            {
                if (!typeof(EPiServer.Core.BlockData).IsAssignableFrom(property.ModelType))
                {
                    continue;
                }

                if (property.LayoutClass == "epi/shell/layout/ParentContainer" &&
                    property.ClientEditingClass == null)
                {
                    property.LayoutClass = "alloy-extendedBlocks/ParentContainer";
                }
            }
        }
    }
}
