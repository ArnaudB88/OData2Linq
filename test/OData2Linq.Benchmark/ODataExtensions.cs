
namespace OData2Linq.Benchmark
{
    using Settings;
    using AspNetCoreDefaultQueryConfigurations = Microsoft.AspNetCore.OData.Query.DefaultQueryConfigurations;

    internal static class ODataExtensions
    {
        public static AspNetCoreDefaultQueryConfigurations ToAspNetCoreDefaultQueryConfigurations(this DefaultQueryConfigurations defaultQueryConfigurations)
        {
            return new AspNetCoreDefaultQueryConfigurations
            {
                EnableExpand = defaultQueryConfigurations.EnableExpand,
                EnableSelect = defaultQueryConfigurations.EnableSelect,
                EnableCount = defaultQueryConfigurations.EnableCount,
                EnableOrderBy = defaultQueryConfigurations.EnableOrderBy,
                EnableFilter = defaultQueryConfigurations.EnableFilter,
                MaxTop = defaultQueryConfigurations.MaxTop,
                EnableSkipToken = defaultQueryConfigurations.EnableSkipToken
            };
        }
    }
}