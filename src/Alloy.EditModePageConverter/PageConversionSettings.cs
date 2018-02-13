using System.Collections.Generic;

namespace Alloy.EditModePageConverter
{
    public class PageConversionSettings
    {
        /// <summary>
        /// ID of source PageType
        /// </summary>
        public int FromContentTypeId { get; set; }

        /// <summary>
        /// ID of target PageType
        /// </summary>
        public int ToContentTypeId { get; set; }

        /// <summary>
        /// Property mappings
        /// </summary>
        public List<KeyValuePair<int, int>> PropertyTypeMappings { get; set; }
    }
}
