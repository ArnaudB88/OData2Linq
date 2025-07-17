namespace OData2Linq.Wrappers
{
    public interface IODataQueryWrapper
    {
        IDictionary<string, object?> ToDictionary();
    }
}