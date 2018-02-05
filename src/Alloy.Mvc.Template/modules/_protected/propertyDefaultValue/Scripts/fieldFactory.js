define([
    "dojo",
    "dojo/Stateful",
    "dojo/when",
    "dojo/Deferred",
    "dojo/_base/declare",
    "dojo/dom-construct",
    "dojo/on",

    // dijit
    "dijit/Destroyable",

    // epi
    "epi/routes",
    "epi/shell/MetadataTransformer",
    "epi/dependency",
    "epi/shell/form/Field",
    "epi/shell/form/formFieldRegistry",
    "epi/shell/widget/dialog/Dialog",

    // epi-cms
    "epi-cms/_ContentContextMixin",
    "epi-cms/widget/_HasChildDialogMixin",

    "./propertyVersions"
], function (
    dojo,
    Stateful,
    when,
    Deferred,
    declare,
    domConstruct,
    on,

    // dijit
    Destroyable,

    // epi
    routes,
    MetadataTransformer,
    dependency,
    Field,
    formFieldRegistry,
    Dialog,

    // epi-cms
    _ContentContextMixin,
    _HasChildDialogMixin,

    PropertyVersions
) {
    var factory = formFieldRegistry.get(formFieldRegistry.type.field, "");

    return declare([Stateful, _ContentContextMixin, _HasChildDialogMixin, Destroyable], {
        onRevertClick: function (widget) {

            this._getMetadata(widget.name)
                .then(function (metadataInfo) {
                    var dialog = this._getDialog(widget, metadataInfo.context.name + " - " + metadataInfo.metadata.settings.label);

                    this.propertyVersions.set("widgetSettings", metadataInfo.metadata);
                    this.propertyVersions.set("propertyName", widget.name);
                    this.propertyVersions.set("contentLink", metadataInfo.context.id);
                    this.propertyVersions.set("repositoryKey", widget.params.contentRepositoryKey);
                    this.propertyVersions.set("dataType", [metadataInfo.context.dataType]);
                    this.propertyVersions.set("language", metadataInfo.context.language);

                    dialog.startup();

                    this.isShowingChildDialog = true;
                    dialog.show();
                    dialog.definitionConsumer.setItemProperty(dialog._okButtonName, "disabled", true);
                }.bind(this));
        },

        _getMetadata: function (propertyName) {
            var def = new Deferred();

            when(this.getCurrentContext())
                .then(function (context) {
                    this._metadataManager = this._metadataManager || dependency.resolve("epi.shell.MetadataManager");

                    when(this._metadataManager.getMetadataForType("EPiServer.Core.ContentData",
                            { contentLink: context.id }))
                        .then(function (metadata) {
                            var propertyMetadata = metadata.getPropertyMetadata(propertyName);
                            var transformed = new MetadataTransformer().transformPropertySettings(propertyMetadata);
                            def.resolve({ metadata: transformed, context: context });
                        }.bind(this));

                }.bind(this));

            return def.promise;
        },

        revertablePropertiesFactory: function (widget, parent) {
            var wrapper = factory(widget, parent);

            if (!widget.params.allowRevert || widget.params.readOnly) {
                return wrapper;
            }

            var undoNode = domConstruct
                .toDom("<span class='dijitInline dijitReset dijitIcon epi-iconUndo epi-cursor--pointer' title='Revert to default'></span>");
            domConstruct.place(undoNode, wrapper.labelNode);
            on(undoNode, "click", function () { this.onRevertClick(widget); }.bind(this));
            return wrapper;
        },

        createFactory: function () {
            return {
                type: formFieldRegistry.type.field,
                hint: "",
                factory: this.revertablePropertiesFactory.bind(this)
            }
        },

        _getDialog: function (widget, title) {
            // summary:
            //		Create page tree dialog
            // tags:
            //    protected

            var dialog;

            this.propertyVersions = new PropertyVersions();
            this.own(on(this.propertyVersions, "selectionchanged", function (e) {
                dialog.definitionConsumer.setItemProperty(dialog._okButtonName, "disabled",
                    e.selection.length === 0);
            }.bind(this)));

            dialog = new Dialog({
                title: title,
                dialogClass: "epi-dialog-portrait property-revert-grid-dialog",
                content: this.propertyVersions,
                onShow: function () {
                    this.isShowingChildDialog = true;
                }.bind(this),
                onExecute: function () {
                    var newValue = this.propertyVersions.getSelectedValue();
                    widget.focus();
                    setTimeout(function () {
                        widget.set("value", newValue);
                        widget.onChange(widget.params.defaultValue);
                    }, 0);
                }.bind(this),
                onHide: function () {
                    this.isShowingChildDialog = false;

                    // Return focus to the editor when the dialog closes.
                    //this.focus();
                }.bind(this)
            });

            dialog.own(this.propertyVersions);

            return dialog;
        }
    });
});
