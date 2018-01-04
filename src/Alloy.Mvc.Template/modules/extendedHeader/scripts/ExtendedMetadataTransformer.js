define([
    "dojo/_base/declare",
    "dojo/_base/lang",
    "epi/shell/MetadataTransformer"
],
function (
    declare,
    lang,
    MetadataTransformer) {

    return declare([MetadataTransformer], {

        _getComponentDefinitions: function (propertyDefinitions, groupDefinitions, nameBase, useDefaultValue, readOnly, modelTypeIdentifier) {
            var properties = lang.clone(propertyDefinitions);

            properties.forEach(function (property) {
                if (property.customEditorSettings && property.customEditorSettings.opeHeadingProperty) {
                    property.groupName = "EPiServerCMS_SettingsPanel";
                }
            });

            return this.inherited(arguments, [properties, groupDefinitions, nameBase, useDefaultValue, readOnly, modelTypeIdentifier]);
        }
    });
});
