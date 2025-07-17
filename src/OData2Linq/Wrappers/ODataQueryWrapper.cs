namespace OData2Linq.Wrappers
{
    using System.Collections;
    using Microsoft.AspNetCore.OData.Query.Wrapper;

    internal class ODataQueryWrapper : IODataQueryWrapper
    {
        private ISelectExpandWrapper SelectExpandWrapper { get; }

        public ODataQueryWrapper(ISelectExpandWrapper selectExpandWrapper)
        {
            SelectExpandWrapper = selectExpandWrapper;
        }

        public IDictionary<string, object?> ToDictionary()
        {
            return ToDictionary(SelectExpandWrapper.ToDictionary());
        }

        private static IDictionary<string, object?> ToDictionary(IDictionary<string, object?> dict)
        {
            Dictionary<string, object?> res = new Dictionary<string, object?>(dict.Count);

            foreach(KeyValuePair<string, object?> kv in dict)
            {
                res[kv.Key] = WrapValue(kv.Value);
            }

            return res;
        }

        private static object? WrapValue(object? value)
        {
            if (value is ISelectExpandWrapper wrapper)
            {
                return new ODataQueryWrapper(wrapper);
            }

            if (value is IEnumerable<ISelectExpandWrapper> wrappers)
            {
                return wrappers.Select(IODataQueryWrapper (item) => new ODataQueryWrapper(item));
            }

            if (value is IDictionary<string, object?> dict)
            {
                return ToDictionary(dict);
            }

            if (value is IEnumerable lst && value is not string)
            {
                return lst
                    .Cast<object?>()
                    .Select(WrapValue)
                    .ToList();
            }

            return value;
        }
    }
}