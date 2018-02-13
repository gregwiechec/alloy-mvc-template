using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EPiServer.Core;
using EPiServer.DataAbstraction;

namespace Alloy.EditModePageConverter
{
    public class PropertyMappingsBuilder<TFromType,TToType> where TFromType: PageData where TToType: PageData
    {
        private readonly List<KeyValuePair<int, int>> _propertyTypeMappings = new List<KeyValuePair<int, int>>();

        private readonly IContentTypeRepository _contentTypeRepository;

        private readonly Lazy<PropertyDefinitionCollection> _fromProperties;
        private readonly Lazy<PropertyDefinitionCollection> _toProperties;

        public PropertyMappingsBuilder(IContentTypeRepository contentTypeRepository)
        {
            _contentTypeRepository = contentTypeRepository;

            _fromProperties = new Lazy<PropertyDefinitionCollection>(() => _contentTypeRepository.Load(typeof(TFromType)).PropertyDefinitions);
            _toProperties = new Lazy<PropertyDefinitionCollection>(() => _contentTypeRepository.Load(typeof(TToType)).PropertyDefinitions);
        }

        public PropertyMappingsBuilder<TFromType, TToType> AddAllWithSameNameAnType()
        {
            foreach (var fromProperty in _fromProperties.Value)
            {
                var toProperty = _toProperties.Value.FirstOrDefault(x => x.Name == fromProperty.Name && x.Type.DataType == fromProperty.Type.DataType);
                if (toProperty != null)
                {
                    this._propertyTypeMappings.Add(new KeyValuePair<int, int>(fromProperty.ID, toProperty.ID));
                }
            }
            return this;
        }

        public PropertyMappingsBuilder<TFromType, TToType> AddMapping(Expression<Func<TFromType, object>> propertyFrom, Expression<Func<TToType, object>> propertyTo)
        {
            var fromProperty = ((MemberExpression) propertyFrom.Body).Member.Name;
            var toProperty = ((MemberExpression) propertyTo.Body).Member.Name;

            var fromPropertyDefinition = _fromProperties.Value.Single(x=>x.Name == fromProperty);
            var toPropertyDefinition = _toProperties.Value.Single(x=>x.Name == toProperty);

            _propertyTypeMappings.Add(new KeyValuePair<int, int>(fromPropertyDefinition.ID, toPropertyDefinition.ID));

            return this;
        }

        public PageConversionSettings Build()
        {
            var fromContentType = this._contentTypeRepository.Load(typeof(TFromType));
            var toContentType = this._contentTypeRepository.Load(typeof(TToType));

            var copy = new PageConversionSettings
            {
                FromContentTypeId = fromContentType.ID,
                ToContentTypeId = toContentType.ID,
                PropertyTypeMappings = new List<KeyValuePair<int, int>>(this._propertyTypeMappings)
            };

            foreach (var fromProperty in _fromProperties.Value)
            {
                if (this._propertyTypeMappings.Any(x => x.Key == fromProperty.ID) == false)
                {
                    copy.PropertyTypeMappings.Add(new KeyValuePair<int, int>(fromProperty.ID, 0));
                }
            }
            return copy;
        }
    }
}
