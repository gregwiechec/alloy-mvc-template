define([
    "dojo/_base/declare",
    "dojo/dom-construct",
    "epi/string",
    "epi/shell/layout/SimpleContainer",
    "epi-cms/contentediting/SettingsPanel"
],
function (
    declare,
    domConstruct,
    string,
    SimpleContainer,
    SettingsPanel) {

    return declare([SettingsPanel], {

        buildRendering: function () {
            this.inherited(arguments);

            this._thridColumn = new SimpleContainer();
            this._thridColumn.placeAt(this.domNode, 'last');

            this.originalDetailsWidgetProperties = this.detailsWidgetProperties;

            for (var index in this.additionalProperties) {
                this.additionalProperties[index] = this.additionalProperties[index].map(function (p) {
                    return string.pascalToCamel(p);
                });
            }
        },

        _setContentViewModelAttr: function (contentViewModel) {
            this.inherited(arguments);

            this.detailsWidgetProperties = this.originalDetailsWidgetProperties;

            if (this.additionalProperties[1].length > 0) {
                if (!this.detailsWidgetProperties) {
                    this.detailsWidgetProperties = [];
                }
                var array = this.additionalProperties[1].map(function (p) {
                    return { propertyName: p };
                });
                array.forEach(function (p) {
                    this.detailsWidgetProperties.push(p);
                }, this);
            }

            this._thridColumnProperties = this.additionalProperties[2];

            this._titleCreated = false;
        },

        addChild: function (w) {
            if (this._thridColumnProperties.indexOf(w.name) === -1) {
                this.inherited(arguments);
                return;
            }

            this._addTitle();
            this._thridColumn.addChild(w);
        },

        _addTitle: function () {
            if (this._titleCreated) {
                return;
            }
            var wrapper = domConstruct.create("li", {
                "class": "epi-form-container__section__row epi-form-container__section__row--deatails"
            });
            var titleNode = domConstruct.create("h2", {
                innerHTML: "Additional properties",
                style: {
                    "padding-left": "5px",
                    "background-color": "#dadada",
                    margin: 0
                }
            });
            wrapper.appendChild(titleNode);
            domConstruct.place(wrapper, this._thridColumn.containerNode, "last");
            this._titleCreated = true;
        }
    });
});
