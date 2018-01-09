﻿using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;
 using Alloy.SwitchProperty;

namespace AlloyTemplates.Models.Pages
{
    /// <summary>
    /// Used for the pages mainly consisting of manually created content such as text, images, and blocks
    /// </summary>
    [SiteContentType(GUID = "9CCC8A41-5C8C-4BE0-8E73-520FF3DE8267")]
    [SiteImageUrl(Global.StaticGraphicsFolderPath + "page-type-thumbnail-standard.png")]
    public class StandardPage : SitePageData
    {
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 310)]
        [CultureSpecific]
        public virtual XhtmlString MainBody { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 320)]
        public virtual ContentArea MainContentArea { get; set; }

        [Display(Name= "Use custom layout", GroupName = SystemTabNames.Content, Order = 400)]
        [UIHint(SwitchEditorDescriptor.UIHint)]
        [SwitchSettings(Shape = Shapes.Arc2, TrueStateClass = StateClasses.Warning, FalseStateClass = StateClasses.Info,
            TrueText = "Custom layout", FalseText = "Default layout",
            TrueDescriptionText = "Please note that custom layout requires additional styles configuration", Width = "170px")]
        public virtual bool UseCustomLayout { get; set; }

        [Display(Name = "Show main banner", GroupName = SystemTabNames.Content, Order = 500)]
        [UIHint(SwitchEditorDescriptor.UIHint)]
        [SwitchSettings(Shape = Shapes.Square, TrueText = "Yes", FalseText = "No")]
        public virtual bool ShowMainBanner { get; set; }
    }
}
