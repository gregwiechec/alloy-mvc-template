Use ShowTabWhenPropertyEquals and HideTabWhenPropertyEquals attributes to show Tabs.
Use ShowPropertyWhenValueEquals and HidePropertyWhenValueEquals attributes to show properties.

[SiteContentType(GroupName = Global.GroupNames.Specialized, GUID = "EDDE9ACC-1556-4156-9AA7-B934C01FDA55")]
[SiteImageUrl]
public class BlogPostPage : SitePageData
{
    [ShowTabWhenPropertyEquals(Global.GroupNames.Recipe, (int)BlogPostType.Recipe)]
    public virtual BlogPostType BlogPostType { get; set; }

    [Display(Name = "Show related articles", GroupName = "Related articles", Order = 1)]
    [ShowPropertyWhenValueEquals(nameof(RelatedArticlesHeader), true)]
    public virtual bool ShowRelatedArticles { get; set; }

    [Display(Name = "Header", GroupName = "Related articles", Order = 2)]
    public virtual string RelatedArticlesHeader { get; set; }
}

public enum BlogPostType
{
    Standard,
    Recipe,
    Sponsored
}


To handle more advanced scenarios implement ILayoutVisibilityResolver.

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
