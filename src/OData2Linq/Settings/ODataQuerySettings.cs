namespace OData2Linq.Settings
{
    using AspNetCoreODataQuerySettings = Microsoft.AspNetCore.OData.Query.ODataQuerySettings;
    using AspNetCoreHandleNullPropagationOption = Microsoft.AspNetCore.OData.Query.HandleNullPropagationOption;
    using AspNetAllowedQueryOptions = Microsoft.AspNetCore.OData.Query.AllowedQueryOptions;

    public record ODataQuerySettings
    {
        private AspNetCoreODataQuerySettings AspNetCoreODataQuerySettings { get; }

        public ODataQuerySettings()
        {
            AspNetCoreODataQuerySettings = new AspNetCoreODataQuerySettings();
        }

        public TimeZoneInfo TimeZone
        {
            get => AspNetCoreODataQuerySettings.TimeZone;
            set => AspNetCoreODataQuerySettings.TimeZone = value;
        }

        public int? ModelBoundPageSize
        {
            get => AspNetCoreODataQuerySettings.ModelBoundPageSize;
            set => AspNetCoreODataQuerySettings.ModelBoundPageSize = value;
        }

        public bool EnsureStableOrdering
        {
            get => AspNetCoreODataQuerySettings.EnsureStableOrdering;
            set => AspNetCoreODataQuerySettings.EnsureStableOrdering = value;
        }

        public HandleNullPropagationOption HandleNullPropagation
        {
            get => (HandleNullPropagationOption)AspNetCoreODataQuerySettings.HandleNullPropagation;
            set => AspNetCoreODataQuerySettings.HandleNullPropagation = (AspNetCoreHandleNullPropagationOption)value;
        }

        public bool EnableConstantParameterization
        {
            get => AspNetCoreODataQuerySettings.EnableConstantParameterization;
            set => AspNetCoreODataQuerySettings.EnableConstantParameterization = value;
        }

        public bool EnableCorrelatedSubqueryBuffering
        {
            get => AspNetCoreODataQuerySettings.EnableCorrelatedSubqueryBuffering;
            set => AspNetCoreODataQuerySettings.EnableCorrelatedSubqueryBuffering = value;
        }

        public AllowedQueryOptions IgnoredQueryOptions
        {
            get => (AllowedQueryOptions)AspNetCoreODataQuerySettings.IgnoredQueryOptions;
            set => AspNetCoreODataQuerySettings.IgnoredQueryOptions = (AspNetAllowedQueryOptions)value;
        }

        public AllowedQueryOptions IgnoredNestedQueryOptions
        {
            get => (AllowedQueryOptions)AspNetCoreODataQuerySettings.IgnoredNestedQueryOptions;
            set => AspNetCoreODataQuerySettings.IgnoredNestedQueryOptions = (AspNetAllowedQueryOptions)value;
        }

        public int? PageSize
        {
            get => AspNetCoreODataQuerySettings.PageSize;
            set => AspNetCoreODataQuerySettings.PageSize = value;
        }

        public bool HandleReferenceNavigationPropertyExpandFilter
        {
            get => AspNetCoreODataQuerySettings.HandleReferenceNavigationPropertyExpandFilter;
            set => AspNetCoreODataQuerySettings.HandleReferenceNavigationPropertyExpandFilter = value;
        }

        internal AspNetCoreODataQuerySettings ToAspNetCoreODataQuerySettings()
        {
            return AspNetCoreODataQuerySettings;
        }
    }
}