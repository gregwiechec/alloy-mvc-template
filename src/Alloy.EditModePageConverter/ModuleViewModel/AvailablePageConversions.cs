using System.Collections.Generic;

namespace Alloy.EditModePageConverter.ModuleViewModel
{
    public class AvailablePageConversions
    {
        /// <summary>
        /// Source model type name
        /// </summary>
        public string SourceDataType { get; set; }

        /// <summary>
        /// Key: Target TypeId, Value: Target type display name
        /// </summary>
        public List<KeyValuePair<int, string>> Mappings { get; set; }
    }
}
