namespace OData2Linq.Settings
{
    using AspNetCoreDefaultQueryConfigurations = Microsoft.AspNetCore.OData.Query.DefaultQueryConfigurations;

    public record DefaultQueryConfigurations
    {
        private AspNetCoreDefaultQueryConfigurations AspNetCoreDefaultQueryConfigurations { get; }

        public DefaultQueryConfigurations()
        {
            AspNetCoreDefaultQueryConfigurations = new AspNetCoreDefaultQueryConfigurations();
        }

        public bool EnableExpand
        {
            get => AspNetCoreDefaultQueryConfigurations.EnableExpand;
            set => AspNetCoreDefaultQueryConfigurations.EnableExpand = value;
        }

        public bool EnableSelect
        {
            get => AspNetCoreDefaultQueryConfigurations.EnableSelect;
            set => AspNetCoreDefaultQueryConfigurations.EnableSelect = value;
        }

        public bool EnableCount
        {
            get => AspNetCoreDefaultQueryConfigurations.EnableCount;
            set => AspNetCoreDefaultQueryConfigurations.EnableCount = value;
        }

        public bool EnableOrderBy
        {
            get => AspNetCoreDefaultQueryConfigurations.EnableOrderBy;
            set => AspNetCoreDefaultQueryConfigurations.EnableOrderBy = value;
        }

        public bool EnableFilter
        {
            get => AspNetCoreDefaultQueryConfigurations.EnableFilter;
            set => AspNetCoreDefaultQueryConfigurations.EnableFilter = value;
        }

        public int? MaxTop
        {
            get => AspNetCoreDefaultQueryConfigurations.MaxTop;
            set => AspNetCoreDefaultQueryConfigurations.MaxTop = value;
        }

        public bool EnableSkipToken
        {
            get => AspNetCoreDefaultQueryConfigurations.EnableSkipToken;
            set => AspNetCoreDefaultQueryConfigurations.EnableSkipToken = value;
        }

        internal AspNetCoreDefaultQueryConfigurations ToAspNetCoreDefaultQueryConfigurations()
        {
            return AspNetCoreDefaultQueryConfigurations;
        }
    }
}