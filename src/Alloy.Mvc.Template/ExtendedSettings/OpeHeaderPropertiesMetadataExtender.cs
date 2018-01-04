using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;

namespace AlloyTemplates.ExtendedSettings
{
    [ServiceConfiguration(IncludeServiceAccessor = false)]
    public class OpeHeaderPropertiesMetadataExtender : IMetadataExtender
    {
        public void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            var property =
                metadata.Properties.Cast<ExtendedMetadata>()
                    .FirstOrDefault(p => p.GroupName == SystemTabNames.PageHeader);
            if (property == null)
            {
                return;
            }
            property.GroupSettings.ClientLayoutClass = "extendedHeader/ExtendedSettingPanel";

            var additionalOpeProperties = new Dictionary<int, List<string>>
            {
                [0] = new List<string>(),
                [1] = new List<string>(),
                [2] = new List<string>()
            };

            foreach (var extendedMetadata in metadata.Properties.OfType<ExtendedMetadata>())
            {
                var opeHeadingPropertyAttribute = extendedMetadata.Attributes.OfType<OpeHeadingPropertyAttribute>().FirstOrDefault();
                if (opeHeadingPropertyAttribute == null)
                {
                    continue;
                }

                extendedMetadata.CustomEditorSettings["opeHeadingProperty"] = true;
                additionalOpeProperties[(int)opeHeadingPropertyAttribute.HeaderColumn].Add(extendedMetadata.PropertyName);
            }
            property.GroupSettings.Options["additionalProperties"] = additionalOpeProperties;
        }
    }
}
