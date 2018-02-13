using System.ComponentModel.DataAnnotations;
using AlloyTemplates.Business.Rendering;
using EPiServer.Web;
using EPiServer.Core;

namespace AlloyTemplates.Models.Pages
{
    /// <summary>
    /// Represents contact details for a contact person
    /// </summary>
    [SiteContentType(
        GUID = "5CF1F9F4-0DB5-4F26-913F-C4D8C9829617",
        GroupName = Global.GroupNames.Specialized)]
    [SiteImageUrl(Global.StaticGraphicsFolderPath + "page-type-thumbnail-contact.png")]
    public class CustomerPage : ContactPage
    {
        [Display(GroupName = Global.GroupNames.Contact)]
        public virtual string CustomerId { get; set; }
    }
}
