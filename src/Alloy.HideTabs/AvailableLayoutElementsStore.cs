using System.Web.Mvc;
using Alloy.HideTabs.LayoutVisibilityResolver;
using EPiServer;
using EPiServer.Core;
using EPiServer.Shell.Services.Rest;

namespace Alloy.HideTabs
{
    [RestStore("availableLayoutElementsStore")]
    public class AvailableLayoutElementsStore : RestControllerBase
    {
        private readonly IContentLoader _contentLoader;
        private readonly ILayoutVisibilityResolver _layoutVisibilityResolver;

        public AvailableLayoutElementsStore(IContentLoader contentLoader, CompositeLayoutResolver layoutVisibilityResolver)
        {
            _contentLoader = contentLoader;
            _layoutVisibilityResolver = layoutVisibilityResolver;
        }

        [HttpGet]
        public RestResultBase Get(ContentReference id)
        {
            var content = _contentLoader.Get<IContent>(id);

            var hiddenTabs = _layoutVisibilityResolver.GetHiddenTabs(content);
            var hiddenProperties = _layoutVisibilityResolver.GetHiddenProperties(content);

            return new RestResult
            {
                Data = new
                {
                    HiddenTabs = hiddenTabs,
                    HiddenProperties = hiddenProperties
                }
            };
        }
    }
}
