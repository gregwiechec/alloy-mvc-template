define([
        "dojo/_base/declare",
        "epi/shell/MetadataTransformer"
    ],
    function (
        declare,
        MetadataTransformer
    ) {
        return declare([MetadataTransformer], {
            _createParentContainer: function (property, nameBase, useDefaultValue, readOnly, modelTypeIdentifier) {
                var parentContainer = this.inherited(arguments);
                // we need to set modelType to make AllowedTypes working
                parentContainer.settings.modelType = property.modelType;
                return parentContainer;
            }
        });
    });
