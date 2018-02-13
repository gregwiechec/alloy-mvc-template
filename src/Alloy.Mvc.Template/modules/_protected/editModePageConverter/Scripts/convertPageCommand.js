define([
        "dojo/_base/declare",
        "dojo/_base/json",

        "epi/dependency",

        "epi/shell/DialogService",
        "epi/shell/command/_Command",

        "epi/i18n!epi/cms/nls/admin.convertpagetype"
    ],
    function (
        declare,
        json,

        dependency,

        dialogService,
        _Command,

        resources
    ) {
        return declare([_Command], {
            label: "Convert page",

            canExecute: true,

            isAvailable: true,

            postscript: function () {
                this.inherited(arguments);

                var registry = dependency.resolve("epi.storeregistry");
                this.store = registry.get("alloy.editModePageConverterStore");
            },

            _execute: function () {
                var description = resources.removepropertywarning1 + "<br>" + resources.removepropertywarning2;

                dialogService.confirmation({
                        title: "Warning!",
                        heading: resources.removepropertywarningheading,
                        description: description,
                    }).then(function () {
                        this.store.executeMethod("ConvertPage", this.get("contentLink"), { toContentTypeId: this.get("targetType") })
                            .then(function (result) {
                                dialogService.alert({ title: "Info", description: result }).then(function () {
                                    window.location.reload();
                                });
                            }, function (result) {
                                dialogService.alert({ title: "Error", description: json.fromJson(result.responseText).errorMessage });
                            });
                    }.bind(this));
            }
        });
    });
