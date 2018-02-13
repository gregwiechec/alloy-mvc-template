using System.Collections.Generic;
using EPiServer.ServiceLocation;

namespace Alloy.EditModePageConverter
{
    [Options]
    public class PageConversionOptions
    {
        public List<PageConversionSettings> Settings { get; private set; } = new List<PageConversionSettings>();
    }
}
