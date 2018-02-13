using System.Collections.Generic;
using EPiServer.Framework.Web.Resources;
using EPiServer.Shell.Modules;

namespace Alloy.EditModePageConverter.ModuleViewModel
{
    public class PageConvertViewModel : EPiServer.Shell.Modules.ModuleViewModel
    {
        public PageConvertViewModel(ShellModule module, IClientResourceService clientResourceService) : base(module, clientResourceService)
        {
        }

        public List<AvailablePageConversions> AvailablePagesConversions { get; set; }
    }
}
