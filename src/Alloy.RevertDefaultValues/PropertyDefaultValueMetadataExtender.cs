using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Cms.Shell.UI.UIDescriptors;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;

namespace Alloy.RevertDefaultValues
{
    [ServiceConfiguration(IncludeServiceAccessor = false)]
    public class PropertyDefaultValueMetadataExtender : IMetadataExtender
    {
        private readonly IContentRepository _contentRepository;

        private readonly Type[] DefaultRevertableTypes = new[]
        {
            typeof(string), typeof(CategoryList), typeof(Url), typeof(XhtmlString), typeof(ContentReference)
        };

        public PropertyDefaultValueMetadataExtender(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        public void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            var repositoryKey = getReposiotryKey(metadata);
            foreach (var property in metadata.Properties.Cast<ExtendedMetadata>())
            {
                var allowRevert = property.Attributes.OfType<AllowRevertToDefaultAttribute>().FirstOrDefault();
                if (allowRevert == null)
                {
                    property.EditorConfiguration["allowRevert"] = GetDefaultAllowRevert(property);
                }
                else
                {
                    property.EditorConfiguration["allowRevert"] = allowRevert.AllowRevert;
                    property.EditorConfiguration["contentRepositoryKey"] = repositoryKey;
                }
            }
        }

        private bool GetDefaultAllowRevert(ExtendedMetadata property)
        {
            var propertyModelType = property.ModelType;
            return DefaultRevertableTypes.Any(x => x.IsAssignableFrom(propertyModelType));
        }

        private string getReposiotryKey(ExtendedMetadata metadata)
        {
            var modelType = metadata.Model.GetType();

            if (typeof(IContentMedia).IsAssignableFrom(modelType))
            {
                return MediaRepositoryDescriptor.RepositoryKey;
            }

            if (typeof(BlockData).IsAssignableFrom(modelType))
            {
                return BlockRepositoryDescriptor.RepositoryKey;
            }

            if (typeof(PageData).IsAssignableFrom(modelType))
            {
                return PageRepositoryDescriptor.RepositoryKey;
            }

            return null;
        }
    }
}
