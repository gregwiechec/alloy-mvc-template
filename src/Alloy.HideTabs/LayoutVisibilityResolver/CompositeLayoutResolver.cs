using System.Collections.Generic;
using System.Linq;
using EPiServer.Core;
using EPiServer.ServiceLocation;

namespace Alloy.HideTabs.LayoutVisibilityResolver
{
    [ServiceConfiguration(typeof(CompositeLayoutResolver))]
    public class CompositeLayoutResolver: ILayoutVisibilityResolver
    {
        private readonly IEnumerable<ILayoutVisibilityResolver> _layoutVisibilityResolvers;

        public CompositeLayoutResolver(IEnumerable<ILayoutVisibilityResolver> layoutVisibilityResolvers)
        {
            _layoutVisibilityResolvers = layoutVisibilityResolvers;
        }

        public IEnumerable<string> GetHiddenTabs(IContent content)
        {
            return _layoutVisibilityResolvers.SelectMany(x => x.GetHiddenTabs(content)).Distinct();
        }

        public IEnumerable<string> GetHiddenProperties(IContent content)
        {
            return _layoutVisibilityResolvers.SelectMany(x => x.GetHiddenProperties(content)).Distinct();
        }
    }
}
