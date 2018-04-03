using System.ComponentModel.DataAnnotations;
 using Alloy.GroupingHeader;
using AlloyTemplates.Models.Blocks;

namespace AlloyTemplates.Models.Pages
{
    [SiteContentType(GUID = "43F7F0D6-25DF-4E7F-B2B0-63699249517B")]
    public class TestPage : SitePageData
    {
        [GroupingHeader("Test Test Test Test Test Test Test Test Test Test")]
        [Display(GroupName = "Company", Order = 1)]
        public virtual string TestG11 { get; set; }

        [Display(Name="City", GroupName = "Company", Order = 2)]
        public virtual string TestG12 { get; set; }

        [Display(Name = "State", GroupName = "Company", Order = 3)]
        public virtual string TestG13 { get; set; }

        [GroupingHeader("Contact phones")]
        [Display(Name="Main phone", GroupName = "Company", Order = 4)]
        public virtual string TestG11b { get; set; }

        [Display(Name="Sales department", GroupName = "Company", Order = 5)]
        public virtual string TestG12b { get; set; }

        [Display(Name = "Customer support", GroupName = "Company", Order = 6)]
        public virtual string TestG13b { get; set; }

        [GroupingHeader("Contact emails")]
        [Display(Name="Sales", GroupName = "Company", Order = 7)]
        public virtual string TestG11c { get; set; }

        [Display(Name="Press", GroupName = "Company", Order = 8)]
        public virtual string TestG12c { get; set; }

        [Display(Name="General", GroupName = "Company", Order = 9)]
        public virtual string TestG13c { get; set; }

        /* Tab Group 2 */
        [GroupingHeader("Company adress", "address-group")]
        [Display(Name = "Street", GroupName = "Details", Order = 1)]
        public virtual string TestG21 { get; set; }

        [Display(Name = "City", GroupName = "Details", Order = 2)]
        public virtual string TestG22 { get; set; }

        [Display(Name = "State", GroupName = "Details", Order = 3)]
        public virtual string TestG23 { get; set; }

        [GroupingHeader("Contact emails", "emails-group")]
        [Display(Name = "General", GroupName = "Details", Order = 4)]
        public virtual string TestG21b { get; set; }

        [Display(Name = "Sales", GroupName = "Details", Order = 5)]
        public virtual string TestG22b { get; set; }

        [Display(Name = "Marketing", GroupName = "Details", Order = 6)]
        public virtual string TestG23b { get; set; }

        [GroupingHeader("Press", "press-group")]
        [Display(Name = "Contact 1", GroupName = "Details", Order = 7)]
        public virtual string TestG21c { get; set; }

        [Display(Name = "Contact 2", GroupName = "Details", Order = 8)]
        public virtual string TestG22c { get; set; }

        [Display(Name = "Contact 3", GroupName = "Details", Order = 9)]
        public virtual string TestG23c { get; set; }

        [Display(GroupName = "Contact")]
        public virtual string ContactHeader { get; set; }

        [Display(GroupName = "Contact")]
        public virtual ContactBlock MainContact { get; set; }
    }
}
