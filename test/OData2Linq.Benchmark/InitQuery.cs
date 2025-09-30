using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OData.UriParser;
using OData2Linq.Settings;
using OData2Linq.Tests.SampleData;
using System.ComponentModel.Design;

namespace OData2Linq.Benchmark
{
    public class InitQuery
    {
        private readonly ODataUriResolver _defaultResolver = new StringAsEnumResolver { EnableCaseInsensitive = true };

        private readonly IEdmModel _defaultEdmModel;

        private readonly IQueryable<ClassWithDeepNavigation> _query;

        public InitQuery()
        {
            _query = ClassWithDeepNavigation.CreateQuery();

            var builder = new ODataConventionModelBuilder();
            builder.AddEntityType(typeof(ClassWithDeepNavigation));
            builder.AddEntitySet(typeof(ClassWithDeepNavigation).Name, new EntityTypeConfiguration(new ODataModelBuilder(), typeof(ClassWithDeepNavigation)));
            _defaultEdmModel = builder.GetEdmModel();
        }

        [Benchmark]
        public Tuple<IQueryable, ServiceContainer> LegacyEdmAndContainer()
        {
            var builder = new ODataConventionModelBuilder();
            builder.AddEntityType(typeof(ClassWithDeepNavigation));
            builder.AddEntitySet(typeof(ClassWithDeepNavigation).Name, new EntityTypeConfiguration(new ODataModelBuilder(), typeof(ClassWithDeepNavigation)));
            var edmModel = builder.GetEdmModel();

            var settings = new ODataSettings();

            var container = new ServiceContainer();
            container.AddService(typeof(IEdmModel), edmModel);
            container.AddService(typeof(ODataQuerySettings), settings.QuerySettings);
            container.AddService(typeof(ODataUriParserSettings), settings.ParserSettings);
            container.AddService(typeof(ODataUriResolver), settings.Resolver ?? _defaultResolver);
            container.AddService(typeof(ODataSettings), settings);
            container.AddService(typeof(DefaultQueryConfigurations), settings.DefaultQueryConfigurations);

            return new Tuple<IQueryable, ServiceContainer>(_query, container);
        }

        [Benchmark]
        public Tuple<IQueryable, IServiceProvider> LegacyContainer()
        {
            var edmModel = _defaultEdmModel;

            if (!edmModel.SchemaElements.Any(e => e.SchemaElementKind == EdmSchemaElementKind.EntityContainer))
            {
                throw new ArgumentException("Provided Entity Model have no IEdmEntityContainer", nameof(edmModel));
            }

            var settings = new ODataSettings();

            var container = new ServiceContainer();
            container.AddService(typeof(IEdmModel), edmModel);
            container.AddService(typeof(ODataQuerySettings), settings.QuerySettings);
            container.AddService(typeof(ODataUriParserSettings), settings.ParserSettings);
            container.AddService(typeof(ODataUriResolver), settings.Resolver ?? _defaultResolver);
            container.AddService(typeof(ODataSettings), settings);
            container.AddService(typeof(DefaultQueryConfigurations), settings.DefaultQueryConfigurations);

            return new Tuple<IQueryable, IServiceProvider>(_query, container);
        }

        [Benchmark]
        public Tuple<IQueryable, IServiceProvider> ODataExtension()
        {
            var odataQuery = _query.OData();
            return new Tuple<IQueryable, IServiceProvider>(odataQuery, odataQuery.ServiceProvider);
        }
    }
}
