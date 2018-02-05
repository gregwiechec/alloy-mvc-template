﻿using System.Collections.Generic;
 using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;
 using Alloy.RevertDefaultValues;

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

        [Display(Name="String property 1", GroupName = SystemTabNames.Content, Order = 500)]
        [AllowRevertToDefault]
        public virtual string TestString1 { get; set; }

        [Display(Name="String property 2", GroupName = SystemTabNames.Content, Order = 510)]
        [AllowRevertToDefault]
        [CultureSpecific]
        public virtual string TestString2 { get; set; }

        [Display(Name="Test ContentArea", GroupName = SystemTabNames.Content, Order = 510)]
        [AllowRevertToDefault]
        [CultureSpecific]
        public virtual ContentArea TestContentArea { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            this.TestString1 = "test value 123";
            this.TestContentArea = new ContentArea();
            this.TestContentArea.Items.Add(new ContentAreaItem { ContentLink = ContentReference.StartPage, RenderSettings = new Dictionary<string, object>()});
            this.TestContentArea.Items.Add(new ContentAreaItem { ContentLink = new ContentReference(7), RenderSettings = new Dictionary<string, object>()});
        }
    }
}
