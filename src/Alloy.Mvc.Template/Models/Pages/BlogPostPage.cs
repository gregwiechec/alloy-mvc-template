using System;
using System.ComponentModel.DataAnnotations;
using Alloy.HideTabs;
using Alloy.HideTabs.VisibilityAttributes;
using EPiServer;
using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors.SelectionFactories;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;

namespace AlloyTemplates.Models.Pages
{
    [SiteContentType(GroupName = Global.GroupNames.Specialized, GUID = "EDDE9ACC-1556-4156-9AA7-B934C01FDA55")]
    [SiteImageUrl]
    public class BlogPostPage : SitePageData
    {
        [Display(GroupName = SystemTabNames.Content, Order = 310)]
        [CultureSpecific]
        public virtual XhtmlString MainBody { get; set; }

        [Display(Name = "Blog post type", GroupName = SystemTabNames.Content, Order = 90)]
        [SelectOne(SelectionFactoryType = typeof(BlogPostTypeSelectionFactory))]
        [ShowTabWhenPropertyEquals(Global.GroupNames.Recipe, (int)BlogPostType.Recipe)]
        [ShowTabWhenPropertyEquals(Global.GroupNames.Sponsored, (int)BlogPostType.Sponsored)]
        public virtual BlogPostType BlogPostType { get; set; }

        [Display(Name = "Recipe title", GroupName = Global.GroupNames.Recipe, Order = 500)]

        //[VisilbeWhen(nameof(BlogPostType), BlogPostType.Recipe)]
        public virtual string RecipeTitle { get; set; }

        [Display(Name = "Recipe content", GroupName = Global.GroupNames.Recipe, Order = 510)]
        public virtual XhtmlString RecipeContent { get; set; }

        [Display(Name = "Sponsored content Customer URL", GroupName = Global.GroupNames.Sponsored, Order = 1000)]
        public virtual Url SponsoredContentUrl { get; set; }

        [Display(Name = "Sponsored content Customer Logo", GroupName = Global.GroupNames.Sponsored, Order = 1010)]
        [UIHint(UIHint.Image)]
        public virtual ContentReference SponsoredContentCustomerLogo { get; set; }

        [Display(Name = "Text 1", GroupName = SystemTabNames.Content, Order = 500)]
        [RequiresLayoutRefresh]
        public virtual string Text1 { get; set; }

        [Display(Name = "Text 2", GroupName = SystemTabNames.Content, Order = 501)]
        [RequiresLayoutRefresh]
        public virtual string Text2 { get; set; }

        [Display(Name = "Text 3", GroupName = SystemTabNames.Content, Order = 502)]
        [RequiresLayoutRefresh]
        public virtual string Text3 { get; set; }

        [Display(Name = "Secret property", GroupName = "Secret", Order = 502)]
        public virtual string SecretText { get; set; }

        [Display(Name = "Show related articles", GroupName = "Related articles", Order = 1)]
        [ShowPropertyWhenValueEquals(nameof(RelatedArticlesHeader), true)]
        [ShowPropertyWhenValueEquals(nameof(RelatedArticles), true)]
        public virtual bool ShowRelatedArticles { get; set; }

        [Display(Name = "Header", GroupName = "Related articles", Order = 2)]
        public virtual string RelatedArticlesHeader { get; set; }

        [Display(Name = "Related articles", GroupName = "Related articles", Order = 3)]
        public virtual ContentArea RelatedArticles { get; set; }
    }

    public enum BlogPostType
    {
        Standard,
        Recipe,
        Sponsored
    }

    public class BlogPostTypeSelectionFactory : EnumSelectionFactory
    {
        public BlogPostTypeSelectionFactory() : this(ServiceLocator.Current.GetInstance<LocalizationService>())
        {
        }

        public BlogPostTypeSelectionFactory(LocalizationService localizationService) : base(localizationService)
        {
        }

        protected override string GetStringForEnumValue(int value)
        {
            return ((BlogPostType) value).ToString();
        }

        protected override Type EnumType => typeof(BlogPostType);
    }
}
