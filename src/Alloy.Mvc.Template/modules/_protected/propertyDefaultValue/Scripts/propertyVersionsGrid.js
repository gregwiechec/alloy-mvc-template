define([
    "dojo",
    "dojo/_base/declare",
    "dojo/aspect",
    "dojo/dom-class",
    "dojo/dom-construct",
    "dojo/on",
    "dojo/when",
    "epi",

    // dijit
    "dijit/popup",
    "dijit/TooltipDialog",

    // dgrid
    "dgrid/OnDemandGrid",
    "dgrid/Selection",
    "dgrid/extensions/ColumnHider",
    "epi-cms/dgrid/formatters",

    // epi
    "epi/shell/widget/WidgetFactory",
    "epi-cms/widget/_GridWidgetBase"
], function (
    dojo,
    declare,
    aspect,
    domClass,
    domConstruct,
    on,
    when,
    epi,

    // dijit
    popup,
    TooltipDialog,

    // DGrid
    OnDemandGrid,
    Selection,
    ColumnHider,
    formatters,

    // epi
    WidgetFactory,
    _GridWidgetBase
) {
    return declare([_GridWidgetBase], {
        _widgetFactory: null,

        _commonDrafts: {},

        showAllVersions: false,

        postMixInProperties: function () {
            this.storeKeyName = "alloy.reverPropertyVersionsStore";
            this.ignoreVersionWhenComparingLinks = false;
            this.contextChangeEvent = null;

            this.inherited(arguments);

            this._widgetFactory = new WidgetFactory();
        },

        buildRendering: function () {
            this.inherited(arguments);

            var customGridClass = declare([OnDemandGrid, Selection, ColumnHider]);
            this.grid = new customGridClass({
                columns: {
                    language: {
                        label: epi.resources.header.language,
                        className: "fixed-column"
                    },
                    status: {
                        label: epi.resources.header.status,
                        renderCell: formatters.commonDraft
                    },
                    savedDate: {
                        label: epi.resources.header.saved,
                        formatter: this._localizeDate
                    },
                    savedBy: {
                        label: epi.resources.header.by,
                        formatter: this._createUserFriendlyUsername
                    },
                    value: {
                        className: "fixed-column",
                        renderCell: this._renderPropertyWidget.bind(this)
                    }
                },
                selectionMode: "single",
                selectionEvents: "click,dgrid-cellfocusin",
                sort: [{ attribute: "savedDate", descending: true }],
                select: function (row, toRow, value) {
                    if (row.element && domClass.contains(row.element, "dgrid-equal-item-values")) {
                        return;
                    }
                    if (domClass.contains(row, "dgrid-equal-item-values")) {
                        return;
                    }
                    this.inherited(arguments);
                }
            }, this.domNode);
        },

        startup: function () {
            if (this._started) {
                return;
            }

            this.inherited(arguments);

            this._initWidget();

            this.own(aspect.around(this.grid, "renderRow", this._aroundRenderRow.bind(this)));

            this.fetchData();
        },

        destroy: function () {
            if (this._tooltip) {
                popup.close(this._tooltip);
            }
            this.inherited(arguments);
        },

        fetchData: function () {
            this._setQuery();
        },

        _setQuery: function () {
            var query = {
                contentLink: this.contentLink,
                currentContentLink: this.currentContentLink,
                propertyName: this.propertyName,
                language: this.language,
                showAllVersions: this.showAllVersions
            };

            if (this.grid && this.grid.columns["language"]) {
                this.grid.toggleColumnHiddenState("language", !!this.language);
            }
            this.grid.set("query", query);

            if (!this.grid.store) {
                this.grid.set("store", this.store);
            }
        },

        _initWidget: function () {
            this.dialogContentWrapper = domConstruct.create("div", { "class": "content-children_image-dialog" });

            this._widgetFactory.createWidgets(this.dialogContentWrapper, this.widgetSettings)
                .then(function (widgets) {
                    this.editor = widgets[0];
                    this.own(this.editor);
                }.bind(this));
        },

        _renderPropertyWidget: function (item, value, node, options) {
            if (item.propertyValue === null) {
                domConstruct.create("span", { innerHTML: "[empty]", "class": "revert-empty" }, node, "last");
                return;
            }

            if (epi.areEqual(item.propertyValue, this.propertyValue)) {
                domConstruct.create("span", { innerHTML: "[equal]", "class": "revert-equal" }, node, "last");
                return;
            }

            function showTooltip () {
                this.editor.set("value", item.propertyValue);
                this.editor.set("readOnly", true);

                if (!this._tooltip) {
                    this.own(
                        this._tooltip = new TooltipDialog({
                            content: this.dialogContentWrapper,
                            closable: true,
                            onMouseLeave: function () {
                                popup.close(this._tooltip);
                            }.bind(this)
                        })
                    );
                }
                popup.open({ popup: this._tooltip, around: node });
            }

            var link = domConstruct.create("a", { innerHTML: "preview" }, node, "last");
            this.own(on(link, "click", showTooltip.bind(this)));
        },

        _aroundRenderRow: function (original) {
            return function (item) {
                // If item is a common draft add it to the hash map.
                if (item.isCommonDraft) {
                    var common = this._commonDrafts[item.language];
                    if (common && common !== item.contentLink) {
                        this._removeCommonDraft(common);
                    }
                    this._commonDrafts[item.language] = item.contentLink;
                }

                // Call original method
                var row = original.apply(this.grid, arguments);

                // Add state specific classes
                domClass.toggle(row, "dgrid-equal-item-values", item.propertyValue === null || epi.areEqual(item.propertyValue, this.propertyValue));

                return row;
            }.bind(this);
        },

        _removeCommonDraft: function (reference) {
            if (reference) {
                var item = this.grid.row(reference).data;
                if (item) {
                    item.isCommonDraft = false;
                }
            }
        }
    });
});
