using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Cms.Shell.UI.Rest;
using EPiServer.Cms.Shell.UI.Rest.Internal;
using EPiServer.Cms.Shell.UI.Rest.Models.Internal;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using EPiServer.Framework.Serialization;
using EPiServer.Shell.Services.Rest;

namespace Alloy.RevertDefaultValues.Store
{
    [RestStore("propertyRevertStore")]
    public class PropertyRevertStore : RestControllerBase
    {
        private readonly IContentVersionRepository _contentVersionRepository;
        private readonly LocalizationService _localizationService;
        private readonly IContentRepository _contentRepository;
        private readonly IObjectSerializer _objectSerializer;
        private readonly IMetaDataResolver _metaDataResolver;

        public PropertyRevertStore(IContentVersionRepository contentVersionRepository, LocalizationService localizationService, IContentRepository contentRepository,
            IObjectSerializerFactory objectSerializerFactory, IMetaDataResolver metaDataResolver)
        {
            _contentVersionRepository = contentVersionRepository;
            _localizationService = localizationService;
            _contentRepository = contentRepository;
            _metaDataResolver = metaDataResolver;
            this._objectSerializer = objectSerializerFactory.GetSerializer(KnownContentTypes.Json);
        }

        [HttpGet]
        public RestResultBase Get(ContentReference contentLink, ContentReference currentContentLink, string propertyName, string language, bool showAllVersions, ItemRange range)
        {
            var propertyValue = this._objectSerializer.Serialize(GetPropertyValue(currentContentLink, propertyName));

            var contentVersionStore = new ContentVersionStore(this._contentRepository, this._contentVersionRepository,
                this._localizationService) {ControllerContext = this.ControllerContext};
            var contentVersionResult =
                (RestResult) contentVersionStore.Get(ContentReference.EmptyReference, contentLink.ToReferenceWithoutVersion(), language,
                    Enumerable.Empty<SortColumn>(), string.Empty, new ItemRange() {Start = 0, Total = int.MaxValue});
            var rangedItems = ((IEnumerable<object>) contentVersionResult.Data);
            var result =
                rangedItems.Select(x => this.CreateViewModel((ContentVersionViewModel) x, propertyName, propertyValue));

            if (!showAllVersions)
            {
                // filter empty values
                result = result.Where(x => x.PropertyValue != null);

                // filter all values different from current value
                if (propertyValue != null)
                {
                    result = result.Where(x => _objectSerializer.Serialize(x.PropertyValue) != propertyValue);
                }

                // get first oldest unique value
                result =
                    result.GroupBy(x => _objectSerializer.Serialize(x.PropertyValue)).Select(x => x.OrderBy(r=>r.SavedDate).FirstOrDefault());
            }

            var resultList = result.ToList();

            return new RestResult
            {
                Data = resultList.Skip(range.Start ?? 0).Take(range.Total ?? int.MaxValue),
                Range = new ItemRange
                {
                    Start = contentVersionResult.Range.Start,
                    Total = resultList.Count()
                }
            };
        }

        private object GetPropertyValue(IContent content, string propertyName)
        {
            object value = null;
            var property = content.Property.Get(propertyName);
            if (property == null)
            {
                var metaData = _metaDataResolver.ResolveMetaData(propertyName.ToLowerInvariant());

                //Check if the property is a metadata property
                if (metaData != null)
                {
                    value = metaData.GetValue(content);
                }
            }
            else
            {
                value = property.Value;
            }

            return value;
        }

        private object GetPropertyValue(ContentReference contentLink, string propertyName)
        {
            var content = _contentRepository.Get<IContent>(contentLink);
            return GetPropertyValue(content, propertyName);
        }

        private PropertyRevertContentViewModel CreateViewModel(ContentVersionViewModel model, string propertyName, string propertyValue)
        {
            var content = this._contentRepository.Get<IContent>(model.ContentLink, new CultureInfo(model.Language));
            var contentValue = GetPropertyValue(content, propertyName);
            var viewModel = new PropertyRevertContentViewModel(model)
            {
                PropertyValue = contentValue
            };
            return viewModel;
        }
    }
}
