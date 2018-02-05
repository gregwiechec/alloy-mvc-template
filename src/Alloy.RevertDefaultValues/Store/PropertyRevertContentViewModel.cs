using EPiServer.Cms.Shell.UI.Rest.Models.Internal;

namespace Alloy.RevertDefaultValues.Store
{
    public class PropertyRevertContentViewModel : ContentVersionViewModel
    {
        public PropertyRevertContentViewModel()
        {
        }

        public PropertyRevertContentViewModel(ContentVersionViewModel model)
        {
            this.ContentLink = model.ContentLink;
            this.Language = model.Language;
            this.Name = model.Name;
            this.SavedBy = model.SavedBy;
            this.StatusChangedBy = model.StatusChangedBy;
            this.SavedDate = model.SavedDate;
            this.Uri = model.Uri;
            this.Status = model.Status;
            this.TypeIdentifier = model.TypeIdentifier;
            this.IsCommonDraft = model.IsCommonDraft;
            this.AllowToDelete = model.AllowToDelete;
        }

        public object PropertyValue { get; set; }
    }
}
