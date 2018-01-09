using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;

namespace Alloy.SwitchProperty
{
    [EditorDescriptorRegistration(TargetType = typeof(bool?), UIHint = UIHint)]
    [EditorDescriptorRegistration(TargetType = typeof(bool), UIHint = UIHint)]
    public class SwitchEditorDescriptor: EditorDescriptor
    {
        public const string UIHint = "Switch";

        public SwitchEditorDescriptor()
        {
            ClientEditingClass = "switchProperty/switchEditor";
        }

        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            base.ModifyMetadata(metadata, attributes);

            var switchPropertySettingsAttribute = attributes.OfType<SwitchSettingsAttribute>().FirstOrDefault();
            if (switchPropertySettingsAttribute != null)
            {
                metadata.EditorConfiguration["switchSettings"] = switchPropertySettingsAttribute;
                metadata.CustomEditorSettings["switchSettings2"] = switchPropertySettingsAttribute;
            }
        }
    }
}
