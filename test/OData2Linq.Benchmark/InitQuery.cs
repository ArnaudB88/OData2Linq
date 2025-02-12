﻿using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.AspNetCore.OData.Query.Validator;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OData.UriParser;
using OData2Linq.Tests.SampleData;
using System.ComponentModel.Design;

namespace OData2Linq.Benchmark
{
    public class InitQuery
    {
        private static readonly ODataSimplifiedOptions SimplifiedOptions = new ODataSimplifiedOptions();

        private static ODataUriResolver DefaultResolver = new StringAsEnumResolver { EnableCaseInsensitive = true };

        private static readonly IEdmModel defaultEdmModel;

        private static readonly IQueryable<ClassWithDeepNavigation> query;

        static InitQuery()
        {
            query = ClassWithDeepNavigation.CreateQuery();

            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.AddEntityType(typeof(ClassWithDeepNavigation));
            builder.AddEntitySet(typeof(ClassWithDeepNavigation).Name, new EntityTypeConfiguration(new ODataModelBuilder(), typeof(ClassWithDeepNavigation)));
            defaultEdmModel = builder.GetEdmModel();
        }

        [Benchmark]
        public Tuple<IQueryable, ServiceContainer> LegacyEdmAndContainer()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.AddEntityType(typeof(ClassWithDeepNavigation));
            builder.AddEntitySet(typeof(ClassWithDeepNavigation).Name, new EntityTypeConfiguration(new ODataModelBuilder(), typeof(ClassWithDeepNavigation)));
            var edmModel = builder.GetEdmModel();

            ODataSettings settings = new ODataSettings();

            ServiceContainer container = new ServiceContainer();
            container.AddService(typeof(IEdmModel), edmModel);
            container.AddService(typeof(ODataQuerySettings), settings.QuerySettings);
            container.AddService(typeof(ODataUriParserSettings), settings.ParserSettings);
            container.AddService(typeof(IFilterBinder), new FilterBinder());
            container.AddService(typeof(ODataUriResolver), settings.Resolver ?? DefaultResolver);
            container.AddService(typeof(ODataSimplifiedOptions), SimplifiedOptions);
            container.AddService(typeof(ODataSettings), settings);
            container.AddService(typeof(DefaultQueryConfigurations), settings.DefaultQueryConfigurations);
            container.AddService(typeof(ISelectExpandQueryValidator), new SelectExpandQueryValidator());

            return new Tuple<IQueryable, ServiceContainer>(query, container);
        }

        [Benchmark]
        public Tuple<IQueryable, IServiceProvider> LegacyContainer()
        {
            var edmModel = defaultEdmModel;

            if (edmModel.SchemaElements.Count(e => e.SchemaElementKind == EdmSchemaElementKind.EntityContainer) == 0)
            {
                throw new ArgumentException("Provided Entity Model have no IEdmEntityContainer", nameof(edmModel));
            }

            ODataSettings settings = new ODataSettings();

            ServiceContainer container = new ServiceContainer();
            container.AddService(typeof(IEdmModel), edmModel);
            container.AddService(typeof(ODataQuerySettings), settings.QuerySettings);
            container.AddService(typeof(ODataUriParserSettings), settings.ParserSettings);
            container.AddService(typeof(IFilterBinder), new FilterBinder());
            container.AddService(typeof(ODataUriResolver), settings.Resolver ?? DefaultResolver);
            container.AddService(typeof(ODataSimplifiedOptions), SimplifiedOptions);
            container.AddService(typeof(ODataSettings), settings);
            container.AddService(typeof(DefaultQueryConfigurations), settings.DefaultQueryConfigurations);
            container.AddService(typeof(ISelectExpandQueryValidator), new SelectExpandQueryValidator());

            return new Tuple<IQueryable, IServiceProvider>(query, container);
        }

        [Benchmark]
        public Tuple<IQueryable, IServiceProvider> ODataExtension()
        {
            return new Tuple<IQueryable, IServiceProvider>(query.OData(), new ServiceContainer());
        }
    }
}
