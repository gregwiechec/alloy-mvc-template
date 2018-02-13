using Alloy.EditModePageConverter;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using AlloyTemplates.Models.Pages;
using EPiServer.DataAbstraction;

namespace AlloyTemplates.Business.Initialization
{
    /// <summary>
    /// Module for customizing templates and rendering.
    /// </summary>
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class PageConversionMappingsInitialization : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            var contentTypeRepository = ServiceLocator.Current.GetInstance<IContentTypeRepository>();
            var pageConversionSettingsRepository = ServiceLocator.Current.GetInstance<PageConversionSettingsRepository>();

            // StandardPage: NewsPage, ArticlePage, ProductPage
            var standardToArticle = new PropertyMappingsBuilder<StandardPage,ArticlePage>(contentTypeRepository).AddAllWithSameNameAnType();
            var standardToProduct = new PropertyMappingsBuilder<StandardPage,ProductPage>(contentTypeRepository).AddAllWithSameNameAnType();
            var standardToNews = new PropertyMappingsBuilder<StandardPage,NewsPage>(contentTypeRepository).AddAllWithSameNameAnType();

            // ArticlePage: NewsPage
            var articleToNews = new PropertyMappingsBuilder<ArticlePage, NewsPage>(contentTypeRepository).AddAllWithSameNameAnType();

            pageConversionSettingsRepository.Save(standardToArticle.Build());
            pageConversionSettingsRepository.Save(standardToProduct.Build());
            pageConversionSettingsRepository.Save(standardToNews.Build());
            pageConversionSettingsRepository.Save(articleToNews.Build());

            var employeeToCustomer = new PropertyMappingsBuilder<EmployeePage, CustomerPage>(contentTypeRepository)
                .AddAllWithSameNameAnType()
                .AddMapping(x=>x.EmployeeId, x=>x.CustomerId);

            var customerToEmployee = new PropertyMappingsBuilder<CustomerPage, EmployeePage>(contentTypeRepository)
                .AddAllWithSameNameAnType()
                .AddMapping(x=>x.CustomerId, x=>x.AdditionalInfo);

            pageConversionSettingsRepository.Save(employeeToCustomer.Build());
            pageConversionSettingsRepository.Save(customerToEmployee.Build());
        }

        public void Uninitialize(InitializationEngine context)
        {

        }

        public void Preload(string[] parameters)
        {
        }
    }
}
