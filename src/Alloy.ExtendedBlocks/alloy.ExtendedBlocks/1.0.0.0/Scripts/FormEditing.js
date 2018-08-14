define([
        "dojo/_base/declare",
        "dojo/_base/lang",
        "epi/shell/widget/FormContainer",
        "epi-cms/contentediting/FormEditing",
        "./MetadataTransformer"
    ],
    function (
        declare,
        lang,
        FormContainer,
        FormEditing,
        MetadataTransformer
    ) {
        return declare([FormEditing], {
            _createForm: function () {
                var formContainer = this.inherited(arguments);
                // we need override metadataTransformer
                formContainer.metadataTransformer = new MetadataTransformer({ propertyFilter: formContainer.propertyFilter });
                return formContainer;
            }
        });
    });
