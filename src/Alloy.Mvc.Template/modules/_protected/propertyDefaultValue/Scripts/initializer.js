define([
    "dojo",
    "dojo/_base/declare",
    "dojo/dom-construct",
    "dojo/on",

    "epi/dependency",
    "epi/routes",
    "epi/_Module",
    "epi/shell/form/Field",
    "epi/shell/form/formFieldRegistry",
    "epi/shell/store/Throttle",
    "epi/shell/store/JsonRest",
    "./fieldFactory"
], function (
    dojo,
    declare,
    domConstruct,
    on,

    dependency,
    routes,
    _Module,
    Field,
    formFieldRegistry,
    Throttle,
    JsonRest,
    FieldFactory
) {
    return declare([_Module], {
        initialize: function () {
            this.inherited(arguments);

            this._initializeStore();
            var fieldFactory = new FieldFactory();
            formFieldRegistry.add(fieldFactory.createFactory());
        },

        _initializeStore: function () {
            var registry = this.resolveDependency("epi.storeregistry");

            //Register store
            registry.add("alloy.reverPropertyVersionsStore",
                new Throttle(
                    new JsonRest({
                        target: this._getRestPath("propertyRevertStore"),
                        idProperty: "contentLink"
                    })
                )
            );
        },

        _getRestPath: function (name) {
            return routes.getRestPath({ moduleArea: "propertydefaultvalue", storeName: name });
        }
    });
});
