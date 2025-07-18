//-----------------------------------------------------------------------------
// <copyright file="OrderByQueryOption.cs" company=".NET Foundation">
//      Copyright (c) .NET Foundation and Contributors. All rights reserved.
//      See License.txt in the project root for license information.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.AspNetCore.OData.Query.Validator;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;

namespace Microsoft.AspNetCore.OData.Query;

/// <summary>
/// This defines a $orderby OData query option for querying.
/// </summary>
public class OrderByQueryOption
{
    private OrderByClause _orderByClause;
    private IList<OrderByNode> _orderByNodes;
    private ODataQueryOptionParser _queryOptionParser;

    /// <summary>
    /// Initialize a new instance of <see cref="OrderByQueryOption"/> based on the raw $orderby value and
    /// an EdmModel from <see cref="ODataQueryContext"/>.
    /// </summary>
    /// <param name="rawValue">The raw value for $orderby query. It can be null or empty.</param>
    /// <param name="context">The <see cref="ODataQueryContext"/> which contains the <see cref="IEdmModel"/> and some type information</param>
    /// <param name="queryOptionParser">The <see cref="ODataQueryOptionParser"/> which is used to parse the query option.</param>
    public OrderByQueryOption(string rawValue, ODataQueryContext context, ODataQueryOptionParser queryOptionParser)
    {
        if (context == null)
        {
            throw Error.ArgumentNull("context");
        }

        if (String.IsNullOrEmpty(rawValue))
        {
            throw Error.ArgumentNullOrEmpty("rawValue");
        }

        if (queryOptionParser == null)
        {
            throw Error.ArgumentNull("queryOptionParser");
        }

        Context = context;
        RawValue = rawValue;
        Validator = context.GetOrderByQueryValidator();
        _queryOptionParser = queryOptionParser;
    }

    internal OrderByQueryOption(string rawValue, ODataQueryContext context, string applyRaw, string computeRaw)
    {
        if (context == null)
        {
            throw Error.ArgumentNull("context");
        }

        if (String.IsNullOrEmpty(rawValue))
        {
            throw Error.ArgumentNullOrEmpty("rawValue");
        }

        Context = context;
        RawValue = rawValue;
        Validator = context.GetOrderByQueryValidator();

        Dictionary<string, string> queryOptions = new Dictionary<string, string>();
        queryOptions["$orderby"] = rawValue;
        if (applyRaw != null)
        {
            queryOptions["$apply"] = applyRaw;
        }

        if (computeRaw != null)
        {
            queryOptions["$compute"] = computeRaw;
        }

        _queryOptionParser = new ODataQueryOptionParser(
            context.Model,
            context.ElementType,
            context.NavigationSource,
            queryOptions,
            context.RequestContainer);

        if (context.RequestContainer == null)
        {
            // By default, let's enable the property name case-insensitive
            _queryOptionParser.Resolver = ODataQueryContext.DefaultCaseInsensitiveResolver;
        }

        if (computeRaw != null)
        {
            _queryOptionParser.ParseCompute();
        }

        if (applyRaw != null)
        {
            _queryOptionParser.ParseApply();
        }
    }

    // This constructor is intended for unit testing only.
    internal OrderByQueryOption(string rawValue, ODataQueryContext context)
    {
        if (context == null)
        {
            throw Error.ArgumentNull("context");
        }

        if (String.IsNullOrEmpty(rawValue))
        {
            throw Error.ArgumentNullOrEmpty("rawValue");
        }

        Context = context;
        RawValue = rawValue;
        Validator = context.GetOrderByQueryValidator();
        _queryOptionParser = new ODataQueryOptionParser(
            context.Model,
            context.ElementType,
            context.NavigationSource,
            new Dictionary<string, string> { { "$orderby", rawValue } },
            context.RequestContainer);

        if (context.RequestContainer == null)
        {
            // By default, let's enable the property name case-insensitive
            _queryOptionParser.Resolver.EnableCaseInsensitive = true;
        }
    }

    internal OrderByQueryOption(OrderByQueryOption orderBy)
    {
        Context = orderBy.Context;
        RawValue = orderBy.RawValue;
        Validator = orderBy.Validator;
        _queryOptionParser = orderBy._queryOptionParser;
        _orderByClause = orderBy._orderByClause;
        _orderByNodes = orderBy._orderByNodes;
    }

    /// <summary>
    ///  Gets the given <see cref="ODataQueryContext"/>.
    /// </summary>
    public ODataQueryContext Context { get; private set; }

    /// <summary>
    /// Gets the mutable list of <see cref="OrderByPropertyNode"/> instances for this query option.
    /// </summary>
    public IList<OrderByNode> OrderByNodes
    {
        get
        {
            if (_orderByNodes == null)
            {
                _orderByNodes = OrderByNode.CreateCollection(OrderByClause);
            }
            return _orderByNodes;
        }
    }

    /// <summary>
    ///  Gets the raw $orderby value.
    /// </summary>
    public string RawValue { get; private set; }

    /// <summary>
    /// Gets or sets the OrderBy Query Validator.
    /// </summary>
    public IOrderByQueryValidator Validator { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="ComputeQueryOption"/>.
    /// </summary>
    public ComputeQueryOption Compute { get; set; }

    /// <summary>
    /// Gets the parsed <see cref="OrderByClause"/> for this query option.
    /// </summary>
    public OrderByClause OrderByClause
    {
        get
        {
            if (_orderByClause == null)
            {
                _orderByClause = _queryOptionParser.ParseOrderBy();
                _orderByClause = TranslateParameterAlias(_orderByClause);
            }

            return _orderByClause;
        }
    }

    /// <summary>
    /// Apply the $orderby query to the given IQueryable.
    /// </summary>
    /// <param name="query">The original <see cref="IQueryable"/>.</param>
    /// <returns>The new <see cref="IQueryable"/> after the orderby query has been applied to.</returns>
    public IOrderedQueryable<T> ApplyTo<T>(IQueryable<T> query)
    {
        ODataQuerySettings querySettings = Context.GetODataQuerySettings();
        return ApplyToCore(query, querySettings) as IOrderedQueryable<T>;
    }

    /// <summary>
    /// Apply the $orderby query to the given IQueryable.
    /// </summary>
    /// <param name="query">The original <see cref="IQueryable"/>.</param>
    /// <param name="querySettings">The <see cref="ODataQuerySettings"/> that contains all the query application related settings.</param>
    /// <returns>The new <see cref="IQueryable"/> after the orderby query has been applied to.</returns>
    public IOrderedQueryable<T> ApplyTo<T>(IQueryable<T> query, ODataQuerySettings querySettings)
    {
        return ApplyToCore(query, querySettings) as IOrderedQueryable<T>;
    }

    /// <summary>
    /// Apply the $orderby query to the given IQueryable.
    /// </summary>
    /// <param name="query">The original <see cref="IQueryable"/>.</param>
    /// <returns>The new <see cref="IQueryable"/> after the orderby query has been applied to.</returns>
    public IOrderedQueryable ApplyTo(IQueryable query)
    {
        ODataQuerySettings querySettings = Context.GetODataQuerySettings();
        return ApplyToCore(query, querySettings);
    }

    /// <summary>
    /// Apply the $orderby query to the given IQueryable.
    /// </summary>
    /// <param name="query">The original <see cref="IQueryable"/>.</param>
    /// <param name="querySettings">The <see cref="ODataQuerySettings"/> that contains all the query application related settings.</param>
    /// <returns>The new <see cref="IQueryable"/> after the orderby query has been applied to.</returns>
    public IOrderedQueryable ApplyTo(IQueryable query, ODataQuerySettings querySettings)
    {
        return ApplyToCore(query, querySettings);
    }

    /// <summary>
    /// Validate the orderby query based on the given <paramref name="validationSettings"/>. It throws an ODataException if validation failed.
    /// </summary>
    /// <param name="validationSettings">The <see cref="ODataValidationSettings"/> instance which contains all the validation settings.</param>
    public void Validate(ODataValidationSettings validationSettings)
    {
        if (validationSettings == null)
        {
            throw Error.ArgumentNull("validationSettings");
        }

        if (Validator != null)
        {
            Validator.Validate(this, validationSettings);
        }
    }

    private IOrderedQueryable ApplyToCore(IQueryable query, ODataQuerySettings querySettings)
    {
        if (Context.ElementClrType == null)
        {
            throw Error.NotSupported(SRResources.ApplyToOnUntypedQueryOption, "ApplyTo");
        }

        ICollection<OrderByNode> nodes = OrderByNodes;

        bool alreadyOrdered = false;
        IQueryable querySoFar = query;

        HashSet<object> propertiesSoFar = new HashSet<object>();
        HashSet<string> openPropertiesSoFar = new HashSet<string>();
        bool orderByItSeen = false;

        IOrderByBinder binder = Context.GetOrderByBinder();
        QueryBinderContext binderContext = new QueryBinderContext(Context.Model, querySettings, Context.ElementClrType);
        if (Compute != null)
        {
            binderContext.AddComputedProperties(Compute.ComputeClause.ComputedItems);
        }

        binderContext.EnsureFlattenedProperties(binderContext.CurrentParameter, query);

        foreach (OrderByNode node in nodes)
        {
            if (node is OrderByPropertyNode propertyNode)
            {
                // Use autonomy class to achieve value equality for HasSet.
                var edmPropertyWithPath = new { propertyNode.Property, propertyNode.PropertyPath };
                OrderByDirection direction = propertyNode.Direction;

                // This check prevents queries with duplicate properties (e.g. $orderby=Id,Id,Id,Id...) from causing stack overflows
                if (propertiesSoFar.Contains(edmPropertyWithPath))
                {
                    throw new ODataException(Error.Format(SRResources.OrderByDuplicateProperty, edmPropertyWithPath.PropertyPath));
                }

                propertiesSoFar.Add(edmPropertyWithPath);

                if (propertyNode.OrderByClause != null)
                {
                    querySoFar = AddOrderByQueryForProperty(binder, propertyNode.OrderByClause, querySoFar, binderContext, alreadyOrdered);
                }
                else
                {
                    // could have ensure stable orderby property added
                    querySoFar = ExpressionHelpers.OrderByProperty(querySoFar, Context.Model, edmPropertyWithPath.Property, direction, Context.ElementClrType, alreadyOrdered);
                }

                alreadyOrdered = true;
            }
            else if (node is OrderByOpenPropertyNode openPropertyNode)
            {
                // This check prevents queries with duplicate properties (e.g. $orderby=Id,Id,Id,Id...) from causing stack overflows
                if (openPropertiesSoFar.Contains(openPropertyNode.PropertyName))
                {
                    throw new ODataException(Error.Format(SRResources.OrderByDuplicateProperty, openPropertyNode.PropertyPath));
                }

                openPropertiesSoFar.Add(openPropertyNode.PropertyName);
                Contract.Assert(openPropertyNode.OrderByClause != null);
                querySoFar = AddOrderByQueryForProperty(binder, openPropertyNode.OrderByClause, querySoFar, binderContext, alreadyOrdered);
                alreadyOrdered = true;
            }
            else if (node is OrderByCountNode countNode)
            {
                Contract.Assert(countNode.OrderByClause != null);
                querySoFar = AddOrderByQueryForProperty(binder, countNode.OrderByClause, querySoFar, binderContext, alreadyOrdered);
                alreadyOrdered = true;
            }
            else if (node is OrderByClauseNode clauseNode)
            {
                querySoFar = AddOrderByQueryForProperty(binder, clauseNode.OrderByClause, querySoFar, binderContext, alreadyOrdered);
                alreadyOrdered = true;
            }
            else
            {
                // This check prevents queries with duplicate nodes (e.g. $orderby=$it,$it,$it,$it...) from causing stack overflows
                if (orderByItSeen)
                {
                    throw new ODataException(Error.Format(SRResources.OrderByDuplicateIt));
                }

                querySoFar = ExpressionHelpers.OrderByIt(querySoFar, node.Direction, Context.ElementClrType, alreadyOrdered);
                alreadyOrdered = true;
                orderByItSeen = true;
            }
        }

        return querySoFar as IOrderedQueryable;
    }

    internal List<string> GetOrderByRawValues()
    {
        // If the raw value doesn't contain ',', we don't need to process more.
        // If only one expression (no matter whether it contains ','), we don't need to process more.
        if (!RawValue.Contains(',') || OrderByClause.ThenBy == null)
        {
            return new List<string> { RawValue };
        }

        ODataUri oDataUri = new ODataUri
        {
            ServiceRoot = new Uri("http://localhost"),
            Path = new ODataPath()
        };
        List<string> clauses = new List<string>();
        OrderByClause clause = OrderByClause;
        while (clause != null)
        {
            // Simply remove the 'thenBy'
            OrderByClause newClause = new OrderByClause(null, clause.Expression, clause.Direction, clause.RangeVariable);
            oDataUri.OrderBy = newClause;
            Uri uri = oDataUri.BuildUri(ODataUrlKeyDelimiter.Parentheses);
            string orderbyClause = uri.Query.Substring(10);// the length of "?$orderby=" is 10;
            clauses.Add(Uri.UnescapeDataString(orderbyClause));

            clause = clause.ThenBy;
        }

        return clauses;
    }

    private static IQueryable AddOrderByQueryForProperty(IOrderByBinder orderByBinder,
        OrderByClause orderbyClause, IQueryable querySoFar, QueryBinderContext binderContext, bool alreadyOrdered)
    {
        // Remove Thenby (make Thenby == null) to make sure we only apply the top orderby
        // TODO: need to refactor it later.
        orderbyClause = new OrderByClause(null, orderbyClause.Expression, orderbyClause.Direction, orderbyClause.RangeVariable);

        querySoFar = orderByBinder.ApplyBind(querySoFar, orderbyClause, binderContext, alreadyOrdered);

        return querySoFar;
    }

    private OrderByClause TranslateParameterAlias(OrderByClause orderBy)
    {
        if (orderBy == null)
        {
            return null;
        }

        SingleValueNode orderByExpression = orderBy.Expression.Accept(
            new ParameterAliasNodeTranslator(_queryOptionParser.ParameterAliasNodes)) as SingleValueNode;
        orderByExpression = orderByExpression ?? new ConstantNode(null, "null");

        return new OrderByClause(
            TranslateParameterAlias(orderBy.ThenBy),
            orderByExpression,
            orderBy.Direction,
            orderBy.RangeVariable);
    }
}
