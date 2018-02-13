using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Core;

namespace Alloy.EditModePageConverter
{
    public class PageConversionSettingsRepository
    {
        private readonly PageConversionOptions _pageConversionOptions;
        private volatile Object _lock = new Object();

        public PageConversionSettingsRepository(PageConversionOptions pageConversionOptions)
        {
            _pageConversionOptions = pageConversionOptions;
        }

        public void Save(int fromContentTypeId, int toContentTypeId, List<KeyValuePair<int, int>> propertyTypeMap)
        {
            if (propertyTypeMap == null)
            {
                throw new NullReferenceException("propertyTypeMap has to be set");
            }
            if (fromContentTypeId == toContentTypeId)
            {
                throw new EPiServerException("Can not convert to same page type");
            }

            this.Save(new PageConversionSettings
            {
                FromContentTypeId = fromContentTypeId,
                ToContentTypeId = toContentTypeId,
                PropertyTypeMappings = propertyTypeMap
            });
        }

        public void Save(PageConversionSettings pageConversionSettings)
        {
            var mapping = this._pageConversionOptions.Settings.FirstOrDefault(x => x.FromContentTypeId == pageConversionSettings.FromContentTypeId && x.ToContentTypeId == pageConversionSettings.ToContentTypeId);

            if (mapping != null)
            {
                lock (this._lock)
                {
                    mapping = this._pageConversionOptions.Settings.FirstOrDefault(x => x.FromContentTypeId == pageConversionSettings.FromContentTypeId && x.ToContentTypeId == pageConversionSettings.ToContentTypeId);
                    if (mapping != null)
                    {
                        this._pageConversionOptions.Settings.Remove(mapping);
                    }
                }
            }
            this._pageConversionOptions.Settings.Add(pageConversionSettings);
        }

        public IEnumerable<int> GetConversionsForType(int sourceContentypeId)
        {
            return this._pageConversionOptions.Settings.Where(x => x.FromContentTypeId == sourceContentypeId).Select(x => x.ToContentTypeId);
        }

        public IEnumerable<KeyValuePair<int, int>> GetConversions()
        {
            return this._pageConversionOptions.Settings.Select(x => new KeyValuePair<int, int>(x.FromContentTypeId, x.ToContentTypeId));
        }

        public IEnumerable<KeyValuePair<int, int>> GetMappings(int fromContentTypeId, int toContentTypeId)
        {
            return this._pageConversionOptions.Settings.Single(x => x.FromContentTypeId == fromContentTypeId && x.ToContentTypeId == toContentTypeId).PropertyTypeMappings.ToArray();
        }
    }
}
