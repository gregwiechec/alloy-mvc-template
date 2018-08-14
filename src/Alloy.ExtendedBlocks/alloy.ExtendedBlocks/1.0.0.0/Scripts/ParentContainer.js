define([
    "dojo/_base/declare",
    "dojo/_base/connect",
    "dojo/dom-class",
    "dojo/dom-construct",
    "dojo/dom-style",
    "dojo/topic",
    "dojo/when",

        "epi/dependency",
    "epi/shell/dnd/Target",
    "epi/shell/layout/ParentContainer",

    "xstyle/css!./styles.css"
    ],
    function (
        declare,
        connect,
        domClass,
        domConstruct,
        domStyle,
        topic,
        when,

        dependency,
        Target,
        ParentContainer
    ) {
        return declare([ParentContainer], {
            postCreate: function () {
                this.inherited(arguments);

                var registry = dependency.resolve("epi.storeregistry");
                this.contentDataStore = registry.get("epi.cms.contentdata");

                this.dndOverlay = domConstruct.create("div", { class: "block-drop-area" }, this.domNode);
                domStyle.set(this.domNode, "position", "relative");

                var modelType = this.params.modelType.toLowerCase();
                this.own(this.dropTarget = new Target(this.dndOverlay, {
                    accept: [modelType],
                    isSource: false,
                    createItemOnDrop: false,
                    readOnly: this.readOnly,
                    allowMultipleItems: false
                }));

                this.connect(this.dropTarget, "onDropData", "onDropData");

                topic.subscribe("/dnd/start", this._onDndStart.bind(this));
                topic.subscribe("/dnd/cancel", this._onDndCancel.bind(this));
                topic.subscribe("/dnd/drop", this._onDndDrop.bind(this));
            },

            _onDndStart: function (source, nodes, copy) {
                var accepted = this.dropTarget.checkAcceptance(source, nodes);
                domStyle.set(this.dndOverlay, "display", "block");
                domClass.toggle(this.dndOverlay, "dnd-disabled", !accepted);
            },

            _onDndCancel: function () {
                domStyle.set(this.dndOverlay, "display", "none");
            },

            _onDndDrop: function () {
                domStyle.set(this.dndOverlay, "display", "none");
            },

            onDropData: function (dndData, source, nodes, copy) {
                if (dndData.length !== 1 || !dndData[0].data) {
                    return;
                }

                var contentLink = dndData[0].data.contentLink;
                when(this.contentDataStore.get(contentLink)).then(function (blockData) {
                    this.getChildren().forEach(function (wrapper) {
                        var propertyWrappers = wrapper.getChildren();
                        propertyWrappers.forEach(function (propertyWrapper) {
                            var child = propertyWrapper.getChildren()[0];
                            var propertyName = child.name.replace(this.name + ".", "");
                            if (blockData.properties.hasOwnProperty(propertyName)) {
                                var newValue = blockData.properties[propertyName];
                                child.focus();
                                child.set("value", newValue);
                                child.onChange(newValue);
                            }
                        }, this);
                    }, this);
                }.bind(this));
            }
        });
    });
