using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EPiServer.Cms.Shell.UI.ObjectEditing;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;

namespace AlloyTemplates.ExtendedSettings
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Cms.Shell.InitializableModule))]
    public class ExtendedSettingsInitialization : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            // register new MetadataHandler
            var task = Task.Factory.StartNew(() =>
            {
                var editorRegistry = ServiceLocator.Current.GetInstance<MetadataHandlerRegistry>();
                do
                {
                    Thread.Sleep(1000);
                } while (
                    editorRegistry.GetMetadataHandlers(typeof(ContentData))
                        .OfType<AllowedTypesMetadataExtender>()
                        .FirstOrDefault() == null);
                editorRegistry.RegisterMetadataHandler(typeof(ContentData), new OpeHeaderPropertiesMetadataExtender(), null, EditorDescriptorBehavior.PlaceLast);
            });
            context.InitComplete += (sender, args) => task.Wait();
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}
