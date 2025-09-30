namespace OData2Linq.Helpers
{
    using Microsoft.AspNetCore.OData.Edm;
    using Microsoft.AspNetCore.OData.Query;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OData.Edm;
    using Microsoft.OData.UriParser;
    using OData2Linq.Settings;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    internal class SelectExpandHelper<T>
    {
        private readonly ODataQuery<T> _query;

        private readonly string? _entitySetName;

        private readonly ODataRawQueryOptions _rawValues;
        private readonly ODataQueryContext _context;
        private SelectExpandQueryOption? _selectExpand;

        public SelectExpandHelper(ODataRawQueryOptions rawQueryOptions, ODataQuery<T> query, string? entitySetName)
        {
            ArgumentNullException.ThrowIfNull(rawQueryOptions);
            ArgumentNullException.ThrowIfNull(query);

            _context = new ODataQueryContext(query.EdmModel, query.ElementType, null)
            {
                RequestContainer = query.ServiceProvider
            };
            _query = query;
            _entitySetName = entitySetName;
            _rawValues = rawQueryOptions;

            if (_rawValues.Select != null || _rawValues.Expand != null)
            {
                var raws = new Dictionary<string, string>();
                if (_rawValues.Select != null)
                {
                    raws["$select"] = _rawValues.Select;
                }

                if (_rawValues.Expand != null)
                {
                    raws["$expand"] = _rawValues.Expand;
                }

                ODataQueryOptionParser parser = ODataLinqExtensions.GetParser(_query, _entitySetName, raws);

                _selectExpand = new SelectExpandQueryOption(_rawValues.Select, _rawValues.Expand, _context, parser);
            }
        }
        public void AddAutoSelectExpandProperties()
        {
            bool containsAutoSelectExpandProperties = false;
            var autoExpandRawValue = GetAutoExpandRawValue();
            var autoSelectRawValue = GetAutoSelectRawValue();

            var queryParameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(autoExpandRawValue) && !autoExpandRawValue.Equals(_rawValues.Expand))
            {
                queryParameters["$expand"] = autoExpandRawValue;
                containsAutoSelectExpandProperties = true;
            }
            else
            {
                autoExpandRawValue = _rawValues.Expand;
            }

            if (!string.IsNullOrEmpty(autoSelectRawValue) && !autoSelectRawValue.Equals(_rawValues.Select))
            {
                queryParameters["$select"] = autoSelectRawValue;
                containsAutoSelectExpandProperties = true;
            }
            else
            {
                autoSelectRawValue = _rawValues.Select;

                if (autoSelectRawValue != null)
                {
                    queryParameters["$select"] = autoSelectRawValue;
                }
            }

            if (containsAutoSelectExpandProperties)
            {
                ODataQueryOptionParser parser = ODataLinqExtensions.GetParser(_query, _entitySetName, queryParameters);
                var originalSelectExpand = _selectExpand;

                _selectExpand = new SelectExpandQueryOption(autoSelectRawValue, autoExpandRawValue, _context, parser);
                if (originalSelectExpand != null && originalSelectExpand.LevelsMaxLiteralExpansionDepth > 0)
                {
                    _selectExpand.LevelsMaxLiteralExpansionDepth = originalSelectExpand.LevelsMaxLiteralExpansionDepth;
                }
            }
        }

        public IQueryable Apply(ODataQuery<T> query)
        {
            IQueryable result = query;
            if (_selectExpand != null)
            {
                var tempResult = ApplySelectExpand(result, query.ServiceProvider.GetRequiredService<ODataQuerySettings>());
                if (tempResult != default(IQueryable))
                {
                    result = tempResult;
                }
            }

            return result;
        }

        private TSelect? ApplySelectExpand<TSelect>(TSelect entity, ODataQuerySettings settings)
        {
            var result = default(TSelect);

            if (_selectExpand == null)
                return result;

            var processedClause = _selectExpand.ProcessLevels();
            var newSelectExpand = new SelectExpandQueryOption(_selectExpand.RawSelect, _selectExpand.RawExpand, _selectExpand.Context, processedClause);

            var qsettings = _context.RequestContainer.GetRequiredService<ODataSettings>();

            newSelectExpand.Validate(qsettings.ValidationSettings);

            var type = typeof(TSelect);
            if (type == typeof(IQueryable))
            {
                result = (TSelect)newSelectExpand.ApplyTo((IQueryable)entity, settings);
            }
            else if (type == typeof(object))
            {
                result = (TSelect)newSelectExpand.ApplyTo(entity, settings);
            }


            return result;
        }

        private string GetAutoSelectRawValue()
        {
            var selectRawValue = _rawValues.Select;

            if (string.IsNullOrEmpty(selectRawValue))
            {
                IList<SelectModelPath>? autoSelectProperties = null;

                if (_context.TargetStructuredType != null && _context.TargetProperty != null)
                {
                    autoSelectProperties = _context.Model.GetAutoSelectPaths(_context.TargetStructuredType, _context.TargetProperty);
                }
                else if (_context.ElementType is IEdmStructuredType structuredType)
                {
                    autoSelectProperties = _context.Model.GetAutoSelectPaths(structuredType, null);
                }

                string? autoSelectRawValue = autoSelectProperties == null ? null : string.Join(",", autoSelectProperties.Select(a => a.SelectPath));

                if (!string.IsNullOrEmpty(autoSelectRawValue))
                {
                    if (!string.IsNullOrEmpty(selectRawValue))
                    {
                        selectRawValue = string.Format(CultureInfo.InvariantCulture, "{0},{1}",
                            autoSelectRawValue, selectRawValue);
                    }
                    else
                    {
                        selectRawValue = autoSelectRawValue;
                    }
                }
            }

            return selectRawValue;
        }

        private string GetAutoExpandRawValue()
        {
            var expandRawValue = _rawValues.Expand;

            IList<ExpandModelPath>? autoExpandNavigationProperties = null;
            if (_context.TargetStructuredType != null && _context.TargetProperty != null)
            {
                autoExpandNavigationProperties = _context.Model.GetAutoExpandPaths(_context.TargetStructuredType, _context.TargetProperty, !string.IsNullOrEmpty(_rawValues.Select));
            }
            else if (_context.ElementType is IEdmStructuredType elementType)
            {
                autoExpandNavigationProperties = _context.Model.GetAutoExpandPaths(elementType, null, !string.IsNullOrEmpty(_rawValues.Select));
            }

            string? autoExpandRawValue = autoExpandNavigationProperties == null ? null : string.Join(",", autoExpandNavigationProperties.Select(c => c.ExpandPath));


            if (!string.IsNullOrEmpty(autoExpandRawValue))
            {
                if (!string.IsNullOrEmpty(expandRawValue))
                {
                    expandRawValue = string.Format(CultureInfo.InvariantCulture, "{0},{1}", autoExpandRawValue, expandRawValue);
                }
                else
                {
                    expandRawValue = autoExpandRawValue;
                }
            }
            return expandRawValue;
        }
    }
}