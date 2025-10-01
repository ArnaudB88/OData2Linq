namespace OData2Linq.Settings
{
    [Flags]
    public enum AllowedQueryOptions
    {
        None = 0,
        Filter = 1 << 0,
        Expand = 1 << 1,
        Select = 1 << 2,
        OrderBy = 1 << 3,
        Top = 1 << 4,
        Skip = 1 << 5,
        Count = 1 << 6,
        Format = 1 << 7,
        SkipToken = 1 << 8,
        DeltaToken = 1 << 9,
        Apply = 1 << 10,
        Compute = 1 << 11,
        Search = 1 << 12,
        Supported = Search | Compute | Apply | SkipToken | Format | Count | Skip | Top | OrderBy | Select | Expand | Filter,
        All = Supported | DeltaToken
    }
}