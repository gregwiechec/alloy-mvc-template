define([
    "dojo",
    "dojo/_base/declare",
    "dojo/when",

    "epi/shell/command/_CommandProviderMixin",

    "epi-cms/_ContentContextMixin",
    "epi-cms/command/PopupCommand",

    "./convertPageCommand"
], function (
    dojo,
    declare,
    when,

    _CommandProviderMixin,

    _ContentContextMixin,
    PopupCommand,

    ConvertPageCommand
) {

    return declare([_CommandProviderMixin, _ContentContextMixin], {
        _availablePagesConversions: null,

        constructor: function () {
            this.inherited(arguments);

            this._availablePagesConversions = arguments[0].availablePagesConversions;

            when(this.getCurrentContext())
                .then(function (ctx) {
                    this._resetCommands(ctx);
                }.bind(this));
        },

        contentContextChanged: function (ctx, callerData) {
            this._resetCommands(ctx);
        },

        _resetCommands: function (ctx) {
            this.commands = [];

            var pageConversions = this._availablePagesConversions.filter(function (m) {
                return m.sourceDataType === ctx.dataType;
            });

            if (pageConversions.length !== 1) {
                return;
            }

            pageConversions = pageConversions[0];

            if (pageConversions.mappings.length === 1) {
                var mapping = pageConversions.mappings[0];
                this.add("commands", new ConvertPageCommand({
                    label: "Convert to '" + mapping.value + "'",
                    targetType: mapping.key,
                    contentLink: ctx.id
                }));
            } else if (pageConversions.mappings.length > 1) {
                var nestedCommands = pageConversions.mappings.map(function (m) {
                    return new ConvertPageCommand({
                        label: m.value,
                        category: "context",
                        targetType: m.key,
                        contentLink: ctx.id
                    });
                }, this);

                var popupCommand = new PopupCommand({
                    label: "Conver page",
                    commands: nestedCommands
                });
                popupCommand.category = null;
                this.add("commands", popupCommand);
            }
        }
    });
});
