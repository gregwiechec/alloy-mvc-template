define([
    "dojo/_base/declare",
    "epi-cms/contentediting/OnPageEditing",
    "extendedHeader/ExtendedMetadataTransformer"
],
function (
    declare,
    OnPageEditing,
    ExtendedMetadataTransformer) {

    return declare([OnPageEditing], {

        placeForm: function (form) {
            form.metadataTransformer = new ExtendedMetadataTransformer({
                propertyFilter: function (parent, property) {
                    return property.groupName === "EPiServerCMS_SettingsPanel" ||
                        (property.customEditorSettings && property.customEditorSettings.opeHeadingProperty);
                }
            });

            this.inherited(arguments);
        }
    });
});
