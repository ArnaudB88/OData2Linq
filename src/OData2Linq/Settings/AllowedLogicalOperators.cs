namespace OData2Linq.Settings
{
    [Flags]
    public enum AllowedLogicalOperators
    {
        None = 0,
        Or = 1 << 0,
        And = 1 << 1,
        Equal = 1 << 2,
        NotEqual = 1 << 3,
        GreaterThan = 1 << 4,
        GreaterThanOrEqual = 1 << 5,
        LessThan = 1 << 6,
        LessThanOrEqual = 1 << 7,
        Not = 1 << 8,
        Has = 1 << 9,
        All = Has | Not | LessThanOrEqual | LessThan | GreaterThanOrEqual | GreaterThan | NotEqual | Equal | And | Or
    }
}