using System.Collections.Generic;
using EPiServer.Core;

namespace Alloy.HideTabs.LayoutVisibilityResolver
{
    public interface ILayoutVisibilityResolver
    {
        IEnumerable<string> GetHiddenTabs(IContent content);

        IEnumerable<string> GetHiddenProperties(IContent content);
    }
}
