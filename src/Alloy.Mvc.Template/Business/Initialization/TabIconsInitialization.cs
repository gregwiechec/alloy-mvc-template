using Alloy.EditModeTabIcon;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace AlloyTemplates.Business.Initialization
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class TabIconsInitialization : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            var tabIconResolver = ServiceLocator.Current.GetInstance<TabIconResolver>();

            tabIconResolver.SetIcon("Contact", "epi-iconUser");
            tabIconResolver.SetIcon("Metadata", "epi-iconSearch");
            tabIconResolver.SetIcon("SiteSettings", "epi-iconWebsite");

            tabIconResolver.SetTitleVisible("Advanced", false);
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}
