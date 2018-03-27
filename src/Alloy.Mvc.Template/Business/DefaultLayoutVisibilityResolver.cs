using System.Collections.Generic;
using System.Linq;
using Alloy.HideTabs.LayoutVisibilityResolver;
using AlloyTemplates.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;

namespace AlloyTemplates.Business
{
    [ServiceConfiguration(typeof(ILayoutVisibilityResolver))]
    public class DefaultLayoutVisibilityResolver: ILayoutVisibilityResolver
    {
        public IEnumerable<string> GetHiddenTabs(IContent content)
        {
            var originalType = content.GetOriginalType();

            if (originalType != typeof(BlogPostPage))
            {
                return Enumerable.Empty<string>();
            }

            var blogPostPage = (BlogPostPage)content;

            List<string> hiddenTabs = new List<string>();

            if (blogPostPage.Text1 != "show" || blogPostPage.Text2 != "secret" || blogPostPage.Text3 != "tab")
            {
                hiddenTabs.Add("Secret");
            }

            return hiddenTabs;
        }

        public IEnumerable<string> GetHiddenProperties(IContent content)
        {
            return Enumerable.Empty<string>();
        }
    }
}
