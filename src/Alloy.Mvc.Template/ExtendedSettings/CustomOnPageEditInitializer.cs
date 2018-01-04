using EPiServer.Cms.Shell.UI.UIDescriptors.ViewConfigurations.Internal;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace AlloyTemplates.ExtendedSettings
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class CustomOnPageEditInitializer : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services.Intercept<EPiServer.Shell.ViewConfiguration>(
                (locator, defaultView) => defaultView is OnPageEditing ? new CustomOnPageEditing() : defaultView);
        }
        public void Initialize(InitializationEngine context) { }
        public void Uninitialize(InitializationEngine context) { }
    }

    public class CustomOnPageEditing : OnPageEditing
    {
        public CustomOnPageEditing()
        {
            ViewType = "extendedHeader/ExtendedOnPageEditing";
        }
    }
}
