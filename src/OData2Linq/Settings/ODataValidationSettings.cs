namespace OData2Linq.Settings
{
    using AspNetCoreODataValidationSettings = Microsoft.AspNetCore.OData.Query.Validator.ODataValidationSettings;
    using AspNetAllowedQueryOptions = Microsoft.AspNetCore.OData.Query.AllowedQueryOptions;
    using AspNetAllowedArithmeticOperators = Microsoft.AspNetCore.OData.Query.AllowedArithmeticOperators;
    using AspNetAllowedFunctions = Microsoft.AspNetCore.OData.Query.AllowedFunctions;
    using AspNetAllowedLogicalOperators = Microsoft.AspNetCore.OData.Query.AllowedLogicalOperators;

    public record ODataValidationSettings
    {
        private AspNetCoreODataValidationSettings AspNetCoreODataValidationSettings { get; }

        public ODataValidationSettings()
        {
            AspNetCoreODataValidationSettings = new AspNetCoreODataValidationSettings();
        }

        public AllowedArithmeticOperators AllowedArithmeticOperators
        {
            get => (AllowedArithmeticOperators)AspNetCoreODataValidationSettings.AllowedArithmeticOperators;
            set => AspNetCoreODataValidationSettings.AllowedArithmeticOperators = (AspNetAllowedArithmeticOperators)value;
        }

        public AllowedFunctions AllowedFunctions
        {
            get => (AllowedFunctions)AspNetCoreODataValidationSettings.AllowedFunctions;
            set => AspNetCoreODataValidationSettings.AllowedFunctions = (AspNetAllowedFunctions)value;
        }

        public AllowedLogicalOperators AllowedLogicalOperators
        {
            get => (AllowedLogicalOperators)AspNetCoreODataValidationSettings.AllowedLogicalOperators;
            set => AspNetCoreODataValidationSettings.AllowedLogicalOperators = (AspNetAllowedLogicalOperators)value;
        }

        public ISet<string> AllowedOrderByProperties => AspNetCoreODataValidationSettings.AllowedOrderByProperties;

        public AllowedQueryOptions AllowedQueryOptions
        {
            get => (AllowedQueryOptions)AspNetCoreODataValidationSettings.AllowedQueryOptions;
            set => AspNetCoreODataValidationSettings.AllowedQueryOptions = (AspNetAllowedQueryOptions)value;
        }

        public int MaxOrderByNodeCount
        {
            get => AspNetCoreODataValidationSettings.MaxOrderByNodeCount;
            set => AspNetCoreODataValidationSettings.MaxOrderByNodeCount = value;
        }

        public int MaxAnyAllExpressionDepth
        {
            get => AspNetCoreODataValidationSettings.MaxAnyAllExpressionDepth;
            set => AspNetCoreODataValidationSettings.MaxAnyAllExpressionDepth = value;
        }

        public int MaxNodeCount
        {
            get => AspNetCoreODataValidationSettings.MaxNodeCount;
            set => AspNetCoreODataValidationSettings.MaxNodeCount = value;
        }

        public int? MaxSkip
        {
            get => AspNetCoreODataValidationSettings.MaxSkip;
            set => AspNetCoreODataValidationSettings.MaxSkip = value;
        }

        public int? MaxTop
        {
            get => AspNetCoreODataValidationSettings.MaxTop;
            set => AspNetCoreODataValidationSettings.MaxTop = value;
        }

        public int MaxExpansionDepth
        {
            get => AspNetCoreODataValidationSettings.MaxExpansionDepth;
            set => AspNetCoreODataValidationSettings.MaxExpansionDepth = value;
        }

        internal AspNetCoreODataValidationSettings ToAspNetCoreODataValidationSettings()
        {
            return AspNetCoreODataValidationSettings;
        }
    }
}