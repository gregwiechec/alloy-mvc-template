define([
    "dojo",
    "dojo/_base/declare",

    "epi/_Module",
    "epi/shell/form/formFieldRegistry",
    "./fieldFactory",
    "xstyle/css!./styles.css"
], function (
    dojo,
    declare,

    _Module,
    formFieldRegistry,
    FieldFactory
) {
    return declare([_Module], {
        initialize: function () {
            this.inherited(arguments);

            var fieldFactory = new FieldFactory();
            formFieldRegistry.add(fieldFactory.createFactory());
        }
    });
});
