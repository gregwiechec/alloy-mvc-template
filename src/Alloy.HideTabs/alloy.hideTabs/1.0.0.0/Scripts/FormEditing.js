define([
        "dojo/_base/declare",
        "dojo/dom-class",
        "dojo/on",
        "epi/dependency",
        "epi-cms/contentediting/FormEditing",
        "xstyle/css!./styles.css"
    ],
    function (
        declare,
        domClass,
        on,
        dependency,
        FormEditing
    ) {
        return declare([FormEditing], {
            _store: null,

            _groupMap: null,

            postMixInProperties: function () {
                this.inherited(arguments);

                this._store = this._store || dependency.resolve("epi.storeregistry").get("alloy.availableLayoutElementsStore");

                this._groupMap = {};
            },

            removeForm: function (form) {
                this._groupMap = {};
                return this.inherited(arguments);
            },

            setupEditMode: function () {
                this.inherited(arguments);

                if (this._handle) {
                    this._handle.remove();
                    this._handle = null;
                }

                this._handle = dojo.connect(this.viewModel, "onPropertySaved", function (propertyName, value) {
                    propertyName = propertyName.toLowerCase();
                    var property = this.viewModel.metadata.properties.filter(function (p) {
                        return p.name.toLowerCase() === propertyName;
                    })[0];

                    if (property.customEditorSettings.requiresLayoutRefresh) {
                        this._refreshHiddenElements();
                    }
                }.bind(this));
                this.own(this._handle);
            },

            onSetupEditModeComplete: function () {
                if (!this._form.containerLayout.selectedChildWidget) {
                    return;
                }

                if (this._form.containerLayout.selectedChildWidget.get("disabled")) {
                    for (var group in this._groupMap) {
                        if (this._isTopPanel(group)) {
                            continue;
                        }

                        if (this._groupMap[group].get("disabled") !== true) {
                            this._form.containerLayout.selectChild(this._groupMap[group]);
                            break;
                        }
                    }
                }

                this._hideProperties(this.viewModel.metadata.customEditorSettings.hiddenProperties);
            },

            onGroupCreated: function (groupName, widget, parentGroupWidget) {
                this.inherited(arguments);

                var name = groupName.toLowerCase();
                this._groupMap[name] = widget;

                var hiddenTabs = this.viewModel.metadata.customEditorSettings.hiddenTabs.map(function (x) { return x.toLowerCase(); });
                var tabIsHidden = hiddenTabs.indexOf(name.toLowerCase()) !== -1;
                widget.set("disabled", tabIsHidden);
            },

            _refreshHiddenElements: function () {
                this._store.get(this.viewModel.contentLink)
                    .then(function (response) {
                        // hide tabs
                        var hiddenTabs = response.hiddenTabs.map(function (x) { return x.toLowerCase(); });

                        var firstAvailableTab, shouldSetDefaultTab = false;
                        for (var group in this._groupMap) {
                            if (this._isTopPanel(group)) {
                                continue;
                            }

                            var tabIsHidden = hiddenTabs.indexOf(group.toLowerCase()) !== -1;

                            if (!firstAvailableTab && !tabIsHidden) {
                                firstAvailableTab = this._groupMap[group];
                            }

                            if (this._groupMap[group] === this._form.containerLayout.selectedChildWidget && tabIsHidden) {
                                shouldSetDefaultTab = true;
                            }
                            this._groupMap[group].set("disabled", tabIsHidden);
                        }

                        // in case when current tab is not available we should set default tab
                        if (shouldSetDefaultTab && firstAvailableTab) {
                            this._form.containerLayout.selectChild(firstAvailableTab);
                        }

                        // hide properties
                        this._hideProperties(response.hiddenProperties);
                    }.bind(this));
            },

            _hideProperties: function (hiddenProperties) {
                hiddenProperties = hiddenProperties.map(function (x) { return x.toLowerCase(); });

                for (var propertyName in this._formWidgets) {
                    var propertyIsHidden = hiddenProperties.indexOf(propertyName.toLowerCase()) !== -1;
                    domClass.toggle(this._formWidgets[propertyName].getParent().domNode, "dijitHidden", propertyIsHidden);
                }
            },

            _isTopPanel: function (groupName) {
                // Setting Panel can't be hidden
                return this._groupMap[groupName].params.region === "top";
            }
        });
    });
