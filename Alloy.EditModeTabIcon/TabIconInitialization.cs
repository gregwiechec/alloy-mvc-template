using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;

namespace Alloy.EditModeTabIcon
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Cms.Shell.InitializableModule))]
    public class TabIconInitialization : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            context.InitComplete += (sender, args) =>
            {
                var editorRegistry = ServiceLocator.Current.GetInstance<MetadataHandlerRegistry>();
                editorRegistry.RegisterMetadataHandler(typeof(ContentData), new TabIconMetadataExtender(ServiceLocator.Current.GetInstance<TabIconResolver>()), null, EditorDescriptorBehavior.PlaceLast);
            };
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}
