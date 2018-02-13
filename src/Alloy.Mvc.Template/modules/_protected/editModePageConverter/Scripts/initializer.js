define([
    "dojo",
    "dojo/_base/declare",

    "epi/dependency",
    "epi/routes",
    "epi/_Module",

    "epi/shell/store/Throttle",
    "epi/shell/store/JsonRest",

    "./commandsProvider"
], function (
    dojo,
    declare,

    dependency,
    routes,
    _Module,

    Throttle,
    JsonRest,

    CommandsProvider
) {
    return declare([_Module], {
        availablePagesConversions: [],

        constructor: function(settings) {
            this.inherited(arguments);

            this.availablePagesConversions = settings.availablePagesConversions;
        },

        initialize: function () {
            this.inherited(arguments);

            this._initializeStore();

            var commandregistry = dependency.resolve("epi.globalcommandregistry");
            commandregistry.registerProvider("epi.cms.contentdetailsmenu", new CommandsProvider({
                availablePagesConversions: this.availablePagesConversions
            }));
        },

        _initializeStore: function () {
            var registry = this.resolveDependency("epi.storeregistry");
            registry.add("alloy.editModePageConverterStore",
                new Throttle(
                    new JsonRest({
                        target: this._getRestPath("pageConversionStore"),
                        idProperty: "id"
                    })
                )
            );
        },

        _getRestPath: function (name) {
            return routes.getRestPath({ moduleArea: "editModePageConverter", storeName: name });
        }
    });
});
