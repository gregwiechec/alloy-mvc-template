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
        GUID = "B844E5C8-0117-45CD-9593-4FDDC2AF693D",
        GroupName = Global.GroupNames.Specialized)]
    [SiteImageUrl(Global.StaticGraphicsFolderPath + "page-type-thumbnail-contact.png")]
    public class EmployeePage : ContactPage
    {
        [Display(GroupName = Global.GroupNames.Contact)]
        public virtual string EmployeeId { get; set; }

        [Display(GroupName = Global.GroupNames.Contact)]
        public virtual string AdditionalInfo { get; set; }
    }
}
