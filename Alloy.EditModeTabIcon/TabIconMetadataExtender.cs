using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;

namespace Alloy.EditModeTabIcon
{
    [ServiceConfiguration(IncludeServiceAccessor = false)]
    public class TabIconMetadataExtender : IMetadataExtender
    {
        private readonly TabIconResolver _tabIconResolver;

        public TabIconMetadataExtender(TabIconResolver tabIconResolver)
        {
            _tabIconResolver = tabIconResolver;
        }

        public void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            var properties = metadata.Properties.Cast<ExtendedMetadata>();
            foreach (var property in properties)
            {
                if (property.GroupSettings == null)
                {
                    continue;
                }

                if (property.GroupSettings.ClientLayoutClass != "epi/shell/layout/SimpleContainer")
                {
                    continue;
                }

                var icon = this._tabIconResolver.GetIcon(property.GroupSettings.Name);
                property.GroupSettings.Options["iconClass"] = icon;

                var isTitleVisible = this._tabIconResolver.IsTitleVisible(property.GroupSettings.Name);
                if (!isTitleVisible && string.IsNullOrWhiteSpace(icon) == false)
                {
                    property.GroupSettings.Options["showTitle"] = false;
                }
            }
        }
    }
}
