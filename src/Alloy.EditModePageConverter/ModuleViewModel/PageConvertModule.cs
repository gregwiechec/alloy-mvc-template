using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using EPiServer.DataAbstraction;
using EPiServer.Framework.TypeScanner;
using EPiServer.Framework.Web.Resources;
using EPiServer.ServiceLocation;
using EPiServer.Shell;
using EPiServer.Shell.Modules;

namespace Alloy.EditModePageConverter.ModuleViewModel
{
    public class PageConvertModule: EPiServer.Shell.Modules.ShellModule
    {
        private readonly UIDescriptorRegistry _uiDescriptorRegistry;
        private readonly PageConversionSettingsRepository _pageConversionSettingsRepository;
        private readonly IContentTypeRepository _contentTypeRepository;

        // need to define base constructor with same signature
        public PageConvertModule(string name, string routeBasePath, string resourceBasePath) : this(name, routeBasePath, resourceBasePath,
             ServiceLocator.Current.GetInstance<UIDescriptorRegistry>(),
                ServiceLocator.Current.GetInstance<PageConversionSettingsRepository>(),
                ServiceLocator.Current.GetInstance<IContentTypeRepository>())
        {
        }

        public PageConvertModule(string name, string routeBasePath, string resourceBasePath,
            UIDescriptorRegistry uiDescriptorRegistry, PageConversionSettingsRepository pageConversionSettingsRepository,
            IContentTypeRepository contentTypeRepository) : base(name, routeBasePath, resourceBasePath)
        {
            _uiDescriptorRegistry = uiDescriptorRegistry;
            _pageConversionSettingsRepository = pageConversionSettingsRepository;
            _contentTypeRepository = contentTypeRepository;
        }

        // need to define base constructor with same signature
        public PageConvertModule(string name, string routeBasePath, string resourceBasePath,
            ITypeScannerLookup typeScannerLookup, VirtualPathProvider virtualPathProvider)
            : this(name, routeBasePath, resourceBasePath, typeScannerLookup, virtualPathProvider,
                ServiceLocator.Current.GetInstance<UIDescriptorRegistry>(),
                ServiceLocator.Current.GetInstance<PageConversionSettingsRepository>(),
                ServiceLocator.Current.GetInstance<IContentTypeRepository>())
        {
        }

        public PageConvertModule(string name, string routeBasePath, string resourceBasePath,
            ITypeScannerLookup typeScannerLookup, VirtualPathProvider virtualPathProvider,
            UIDescriptorRegistry uiDescriptorRegistry, PageConversionSettingsRepository pageConversionSettingsRepository,
            IContentTypeRepository contentTypeRepository)
            : base(name, routeBasePath, resourceBasePath, typeScannerLookup, virtualPathProvider)
        {
            _uiDescriptorRegistry = uiDescriptorRegistry;
            _pageConversionSettingsRepository = pageConversionSettingsRepository;
            _contentTypeRepository = contentTypeRepository;
        }

        public override EPiServer.Shell.Modules.ModuleViewModel CreateViewModel(ModuleTable moduleTable, IClientResourceService clientResourceService)
        {
            var viewModel = new PageConvertViewModel(this, clientResourceService)
            {
                AvailablePagesConversions = GetPageConvertMappings()
            };
            return viewModel;
        }

        private List<AvailablePageConversions> GetPageConvertMappings()
        {
            var conversions = _pageConversionSettingsRepository.GetConversions();
            var result = new List<AvailablePageConversions>();

            var contentTypes = new Dictionary<int, PageType>();

            foreach (var conversion in conversions)
            {
                if (!contentTypes.ContainsKey(conversion.Key))
                {
                    contentTypes[conversion.Key] = (PageType)this._contentTypeRepository.Load(conversion.Key);
                }
                if (!contentTypes.ContainsKey(conversion.Value))
                {
                    contentTypes[conversion.Value] = (PageType)this._contentTypeRepository.Load(conversion.Value);
                }
                var sourcePageType = contentTypes[conversion.Key];
                var targetPageType = contentTypes[conversion.Value];

                var sourceDataType = GetTypeIdentifier(sourcePageType.ModelType);
                var mappingResult = result.FirstOrDefault(x => x.SourceDataType == sourceDataType);
                if (mappingResult == null)
                {
                    mappingResult = new AvailablePageConversions
                    {
                        SourceDataType = sourceDataType,
                        Mappings = new List<KeyValuePair<int, string>>()
                    };
                    result.Add(mappingResult);
                }
                mappingResult.Mappings.Add(new KeyValuePair<int, string>(conversion.Value,
                    targetPageType.LocalizedName ?? targetPageType.Name));
            }
            return result;
        }

        private string GetTypeIdentifier(Type modelType)
        {
            return _uiDescriptorRegistry.GetTypeIdentifiers(modelType).FirstOrDefault();
        }
    }
}
