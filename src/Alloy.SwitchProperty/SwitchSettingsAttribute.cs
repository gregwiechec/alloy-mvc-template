using System;

namespace Alloy.SwitchProperty
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SwitchSettingsAttribute: Attribute
    {
        public string Shape { get; set; }

        public string TrueStateClass { get; set; }

        public string TrueText { get; set; }

        public string TrueDescriptionText { get; set; }

        public string FalseText { get; set; }

        public string FalseDescriptionText { get; set; }

        public string FalseStateClass { get; set; }

        /// <summary>
        /// width style value (like "150px");
        /// </summary>
        public string Width { get; set; }
    }
}
