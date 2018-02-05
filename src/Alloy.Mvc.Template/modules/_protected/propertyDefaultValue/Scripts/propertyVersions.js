define([
    "dojo",
    "dojo/_base/declare",
    "dojo/when",
    "dojo/dom-style",
    "dojo/dom-construct",
    "dojo/dom-geometry",
    "dojo/on",
    "epi/dependency",

    // dijit
    "dijit/layout/_LayoutWidget",
    "dijit/_TemplatedMixin",
    "dijit/_WidgetsInTemplateMixin",

    "propertyDefaultValue/propertyVersionsGrid",

    "dojo/text!./propertyVersions.html",

    "epi-cms/contentediting/editors/SelectionEditor",
    "epi-cms/widget/ContentSelector",

    "xstyle/css!./styles.css"
], function (
    // dojo
    dojo,
    declare,
    when,
    domStyle,
    domConstruct,
    domGeometry,
    on,
    dependency,

    // dijit
    _LayoutWidget,
    _TemplatedMixin,
    _WidgetsInTemplateMixin,

    PropertyVersionsGrid,

    template
) {
    return declare([_LayoutWidget, _TemplatedMixin, _WidgetsInTemplateMixin], {

        store: null,

        templateString: template,

        postMixInProperties: function () {
            this.inherited(arguments);

            this._contentRepositoryDescriptors = dependency.resolve("epi.cms.contentRepositoryDescriptors");
        },


        buildRendering: function () {
            this.inherited(arguments);

            this._languageStore = this._languageStore || dependency.resolve("epi.storeregistry").get("epi.cms.language");
            this._initializeLanguages();

            this.own(on(this.grid, "selectionchanged", function (e) {
                this.emit("selectionChanged", { selection: e.selection });
            }.bind(this)));

            this.contentSelector.set("roots", ["1"]); //TODO: read roots from content
            domStyle.set(this.contentSelector.clearButton, "display", "none");

            this.own(on(this.contentSelector, "change", this._refreshGrid.bind(this)),
                on(this.languageSelector, "change", this._refreshGrid.bind(this)),
                on(this.showAllVersions, "change", this._refreshGrid.bind(this))
            );
        },

        getSelectedValue: function () {
            var item = this.grid.get("selection")[0];
            return item.propertyValue;
        },

        _setWidgetSettingsAttr: function (value) {
            this.grid.widgetSettings = value;
        },

        _setPropertyNameAttr: function (value) {
            this.grid.propertyName = value;
        },

        _setContentLinkAttr: function (value) {
            this.grid.set("currentContentLink", value);
            this.grid.set("contentLink", value);
            this.contentSelector.set("value", value);
        },

        _setRepositoryKeyAttr: function (value) {
            var settings = this._contentRepositoryDescriptors.get(value);
            if (settings) {
                this.contentSelector.set("roots", settings.roots);
                this.contentSelector.set("searchArea", settings.searchArea);
            }
        },

        _setDataTypeAttr: function (value) {
            this.contentSelector.set("allowedTypes", value);
        },

        _setLanguageAttr: function (value) {
            this.grid.language = value;
            this._initialLanguage = value;
        },

        layout: function () {
            var cb = this._contentBox, ocb = this._oldContentBox;

            if (!cb) {
                return;
            }

            if (!ocb || cb.w !== ocb.w || cb.h !== ocb.h) {
                this._oldContentBox = cb;
                this._relayout();
            }
        },

        _relayout: function () {
            var gridHeight = domGeometry.getMarginBox(this.containerNode).h - domGeometry.getMarginBox(this.filtersContainer).h - 10 /* bottom margin */;

            domGeometry.setMarginBox(this.grid.domNode, {
                h: gridHeight
            });
        },

        _initializeLanguages: function () {
            when(this._languageStore.query()).then(function (languages) {
                var selections = languages.map(function (l) {
                    return { text: l.cultureName, value: l.languageId };
                });
                selections.splice(0, 0, { text: "All", value: null });
                this.languageSelector.set("selections", selections);
                this.languageSelector.set("value", this._initialLanguage);
            }.bind(this));
        },

        _refreshGrid: function () {
            this.grid.contentLink = this.contentSelector.get("value");
            this.grid.language = this.languageSelector.get("value");
            this.grid.showAllVersions = this.showAllVersions.get("checked");
            this.grid.fetchData();
        }
    });
});
