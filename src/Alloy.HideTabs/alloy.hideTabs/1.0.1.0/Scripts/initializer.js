define([
    "dojo",
    "dojo/_base/declare",

    "epi/dependency",
    "epi/routes",
    "epi/_Module",

    "epi/shell/store/Throttle",
    "epi/shell/store/JsonRest"
], function (
    dojo,
    declare,

    dependency,
    routes,
    _Module,

    Throttle,
    JsonRest
) {
    return declare([_Module], {
        initialize: function () {
            this.inherited(arguments);

            this._initializeStore();
        },

        _initializeStore: function () {
            var registry = this.resolveDependency("epi.storeregistry");
            registry.add("alloy.availableLayoutElementsStore",
                new Throttle(
                    new JsonRest({
                        target: this._getRestPath("availableLayoutElementsStore")
                    })
                )
            );
        },

        _getRestPath: function (name) {
            return routes.getRestPath({ moduleArea: "alloy.hideTabs", storeName: name });
        }
    });
});
