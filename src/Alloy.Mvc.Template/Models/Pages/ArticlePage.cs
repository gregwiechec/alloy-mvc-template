using System.ComponentModel.DataAnnotations;
using AlloyTemplates.ExtendedSettings;

namespace AlloyTemplates.Models.Pages
{
    /// <summary>
    /// Used primarily for publishing news articles on the website
    /// </summary>
    [SiteContentType(
        GroupName = Global.GroupNames.News,
        GUID = "AEECADF2-3E89-4117-ADEB-F8D43565D2F4")]
    [SiteImageUrl(Global.StaticGraphicsFolderPath + "page-type-thumbnail-article.png")]
    public class ArticlePage : StandardPage
    {
        [Display(Name = "Show footer")]
        [OpeHeadingProperty]
        public virtual bool ShowFooter { get; set; }

        [Display(Name = "Show header")]
        [OpeHeadingProperty(HeaderColumn.Third)]
        public virtual bool ShowHeader { get; set; }

        [Display(Name = "Number of articles")]
        [OpeHeadingProperty]
        public virtual int NumberOfFeaturedArticles { get; set; }

        [Display(Name = "Extra checkbox")]
        [OpeHeadingProperty(HeaderColumn.First)]
        public virtual bool ExtraCheckbox { get; set; }

        [Display(Name = "Another extra checkbox")]
        [OpeHeadingProperty(HeaderColumn.First)]
        public virtual bool AnotherExtraCheckbox { get; set; }
    }
}
