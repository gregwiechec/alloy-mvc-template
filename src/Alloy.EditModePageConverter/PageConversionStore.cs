using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Logging.Compatibility;
using EPiServer.Shell;
using EPiServer.Shell.Services.Rest;
using EPiServer.Validation;

namespace Alloy.EditModePageConverter
{
    [RestStore("pageConversionStore")]
    public class PageConversionStore : RestControllerBase
    {
        private readonly IContentLoader _contentLoader;
        private readonly IContentTypeRepository _contentTypeRepository;
        private readonly PageConversionSettingsRepository _pageConversionSettingsRepository;
        private static readonly ILog _log = LogManager.GetLogger(typeof(PageConversionStore));

        public PageConversionStore(IContentLoader contentLoader, IContentTypeRepository contentTypeRepository,
            PageConversionSettingsRepository pageConversionSettingsRepository, UIDescriptorRegistry uiDescriptorRegistry)
        {
            _contentLoader = contentLoader;
            _contentTypeRepository = contentTypeRepository;
            _pageConversionSettingsRepository = pageConversionSettingsRepository;
        }

        public ActionResult ConvertPage(ContentReference id, int toContentTypeId)
        {
            IContent content;
            try
            {
                content = this._contentLoader.Get<IContent>(id);
            }
            catch (ContentNotFoundException)
            {
                return new RestStatusCodeResult(HttpStatusCode.NotFound, "Cannot find item with ContentLink " + id);
            }

            var mappings = _pageConversionSettingsRepository.GetMappings(content.ContentTypeID, toContentTypeId);

            var from = (PageType) _contentTypeRepository.Load(content.ContentTypeID);
            var to = (PageType) _contentTypeRepository.Load(toContentTypeId);
            string result;
            try
            {
                result = PageTypeConverter.Convert((PageReference) content.ContentLink, from, to, mappings.ToList(),
                    false, false);
            }
            catch (Exception e)
            {
                _log.Error("Conversion failed", e);
                return new RestStatusCodeResult(HttpStatusCode.Conflict)
                {
                    Data = new
                    {
                        Severity = ValidationErrorSeverity.Error,
                        ErrorMessage = e.Message
                    }
                };
            }

            _log.Warn("Page converted by editor " + result);

            return this.Rest("The page was converted");
        }
    }
}
