﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.TestCommon;
using Microsoft.AspNetCore.OData.Tests.Models;
using Microsoft.AspNetCore.OData.Tests.Query.Models;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace Microsoft.AspNetCore.OData.Tests.Query.Validator
{
    internal static class ValidationTestHelper
    {
        internal static ODataQueryContext CreateCustomerContext()
        {
            return CreateCustomerContext(true);
        }

        internal static ODataQueryContext CreateCustomerContext(bool setRequestContainer)
        {
            ODataQueryContext context = new ODataQueryContext(GetCustomersModel(), typeof(QueryCompositionCustomer), null);
            if (setRequestContainer)
            {
                context.RequestContainer = new MockServiceProvider();
            }

            context.DefaultQuerySettings.EnableOrderBy = true;
            context.DefaultQuerySettings.MaxTop = null;
            return context;
        }

        internal static ODataQueryContext CreateProductContext()
        {
            return new ODataQueryContext(GetProductsModel(), typeof(Product));
        }

        internal static ODataQueryContext CreateDerivedProductsContext()
        {
            ODataQueryContext context = new ODataQueryContext(GetDerivedProductsModel(), typeof(Product), null);
            context.RequestContainer = new MockServiceProvider();
            context.DefaultQuerySettings.EnableFilter = true;
            return context;
        }

        private static IEdmModel GetCustomersModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<QueryCompositionCustomer>("Customer");
            builder.EntityType<QueryCompositionCustomerBase>();
            return builder.GetEdmModel();
        }

        private static IEdmModel GetProductsModel()
        {
            var builder = GetProductsBuilder();
            return builder.GetEdmModel();
        }

        private static IEdmModel GetDerivedProductsModel()
        {
            var builder = GetProductsBuilder();
            builder.EntitySet<Product>("Product");
            builder.EntityType<DerivedProduct>().DerivesFrom<Product>();
            builder.EntityType<DerivedCategory>().DerivesFrom<Category>();
            return builder.GetEdmModel();
        }

        private static ODataConventionModelBuilder GetProductsBuilder()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Product>("Product");
            return builder;
        }
    }
}
