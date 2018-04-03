define([
    "dojo",
    "dojo/Stateful",
    "dojo/when",
    "dojo/Deferred",
    "dojo/_base/declare",
    "dojo/dom-construct",
    "dojo/on",

    "dojo/aspect",

    // dijit
    "dijit/Destroyable",

    // epi
    "epi/shell/form/Field",
    "epi/shell/form/formFieldRegistry"
], function (
    dojo,
    Stateful,
    when,
    Deferred,
    declare,
    domConstruct,
    on,

    aspect,

    // dijit
    Destroyable,

    // epi
    Field,
    formFieldRegistry
) {
    var factory = formFieldRegistry.get(formFieldRegistry.type.field, "");

    return declare([Stateful, Destroyable], {
        _propertyWithHeaderFactory: function (widget, parent) {
            var wrapper = factory(widget, parent);

            if (!widget.params.groupingHeader) {
                return wrapper;
            }
            aspect.after(wrapper, "startup", function () {
                var groupingHeader = widget.params.groupingHeader;
                var extraClass = "grouping-header";
                if (groupingHeader.className) {
                    extraClass += " " + groupingHeader.className;
                }
                var rowNode = domConstruct.create("li", {
                    "class": "epi-form-container__section__row epi-form-container__section__row--field " + extraClass
                });
                var headerNode = domConstruct.create(groupingHeader.tag, {
                    innerHTML: widget.params.groupingHeader.title
                });
                domConstruct.place(headerNode, rowNode);
                domConstruct.place(rowNode, wrapper.domNode, "before");
            }.bind(this));

            return wrapper;
        },

        createFactory: function () {
            return {
                type: formFieldRegistry.type.field,
                hint: "",
                factory: this._propertyWithHeaderFactory.bind(this)
            }
        }
    });
});
