define([
    "dojo/_base/declare",
    "dojo/dom-class",
    "dojo/dom-style",
    "dijit/_Widget",
    "dijit/_TemplatedMixin",
    "dijit/_WidgetsInTemplateMixin",
    "./dojox/mobile/Switch",
    "xstyle/css!./dojox/mobile/themes/base.css",
    "xstyle/css!./styles.css"
],
    function (
        declare,
        domClass,
        domStyle,
        _Widget,
        _TemplatedMixin,
        _WidgetsInTemplateMixin,
        Switch) {

        return declare([_Widget, _TemplatedMixin, _WidgetsInTemplateMixin], {
            templateString:
                "<div class=\"dijit dijitReset dijitInline\"><div data-dojo-type=\"switchProperty/dojox/mobile/Switch\" style=\"width: \" data-dojo-attach-point=\"switch\"></div><span class=\"additional-text epi-secondaryText\" data-dojo-attach-point=\"descriptionNode\"></span></div>",

            value: null,

            _leftValue: "on",

            postMixInProperties: function () {
                this.inherited(arguments);
            },

            buildRendering: function () {
                // have to set width style directly in template, because updating width later has no effect
                if (this.switchSettings && this.switchSettings.width) {
                    this.templateString = this.templateString.replace("style=\"width: ", "style=\"width: " + this.switchSettings.width);
                } else {
                    this.templateString = this.templateString.replace("style=\"width: \"", "");
                }

                this.inherited(arguments);

                this._setupSwitch();

                this.switch.set("value", "off");

                this.switch.onTouchStart = function () { };
                this.switch.onTouchMove = function () { };
                this.switch.onTouchEnd = function () { };

                this.own(
                    this.switch.on("stateChanged", function (newState) {
                        var isChecked = newState === "on";
                        this._setDescription(isChecked);
                        this._onChange(isChecked);
                    }.bind(this))
                );
            },

            _setDescription: function (value) {
                if (this.switchSettings) {
                    this.descriptionNode.innerText = value
                        ? this.switchSettings.trueDescriptionText || ""
                        : this.switchSettings.falseDescriptionText || "";
                }
            },

            _setupSwitch: function () {
                if (!this.switchSettings) {
                    return;
                }

                if (this.switchSettings.shape) {
                    //this.switch.set("shape", this.switchSettings.shape); // setter has no effect
                    domClass.remove(this.switch.domNode, "mblSwDefaultShape");
                    domClass.add(this.switch.domNode, this.switchSettings.shape);
                }

                if (this.switchSettings.trueText) {
                    this.switch.set("leftLabel", this.switchSettings.trueText);
                }

                if (this.switchSettings.trueStateClass) {
                    domClass.add(this.switch.domNode, this.switchSettings.trueStateClass + "Left");
                }

                if (this.switchSettings.falseText) {
                    this.switch.set("rightLabel", this.switchSettings.falseText);
                }

                if (this.switchSettings.falseStateClass) {
                    domClass.add(this.switch.domNode, this.switchSettings.falseStateClass + "Right");
                }
            },

            _setValueAttr: function (value) {
                this.value = value;
                var state = !!value ? "on" : "off";
                this.switch.set("value", state);
            },

            _setReadOnlyAttr: function (readOnly) {
                this.switch.set("readOnly", readOnly);
            },

            _getReadOnlyAttr: function () {
                return this.switch.set("readOnly");
            },

            _getValueAttr: function () {
                return this.switch.get("value") === "on";
            },

            _onChange: function (value) {
                this.value = value;
                this.onChange(value);
            },

            onChange: function (value) {
            },

            focus: function () {
                this.switch.focus();
            }
        });
    });
