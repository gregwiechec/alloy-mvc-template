using EPiServer.Cms.Shell.UI.UIDescriptors.ViewConfigurations.Internal;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace Alloy.ExtendedBlocks
{
    public class ExtendedFormEditing: FormEditing
    {
        [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
        public class CustomOnPageEditInitializer : IConfigurableModule
        {
            public void ConfigureContainer(ServiceConfigurationContext context)
            {
                context.Services.Intercept<EPiServer.Shell.ViewConfiguration>(
                    (locator, defaultView) => defaultView is FormEditing ? new CustomFormEditing() : defaultView);
            }
            public void Initialize(InitializationEngine context) { }
            public void Uninitialize(InitializationEngine context) { }
        }

        public class CustomFormEditing : FormEditing
        {
            public CustomFormEditing()
            {
                ViewType = "alloy-extendedBlocks/FormEditing";
            }
        }

        /* [ServiceConfiguration(typeof(ViewConfiguration))]
         public class ExtendedFormEditing : OnPageEditing
         {
             public CustomFormEditing()
             {
                 this.ViewType = "alloy-extended-blocks/FormEditing";
                 this.SortOrder = base.SortOrder + 1;
             }
         }*/
    }
}
