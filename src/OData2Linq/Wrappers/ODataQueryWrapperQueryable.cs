namespace OData2Linq.Wrappers
{    
    using Microsoft.AspNetCore.OData.Query.Wrapper;
    using System.Collections;
    using System.Linq.Expressions;

    internal class ODataQueryWrapperQueryable : IQueryable<IODataQueryWrapper>
    {
        private IQueryable<ISelectExpandWrapper> InnerQuery { get; }

        public ODataQueryWrapperQueryable(IQueryable<ISelectExpandWrapper> innerQuery)
        {
            InnerQuery = innerQuery;
        }

        public IEnumerator<IODataQueryWrapper> GetEnumerator()
        {
            return InnerQuery
                .Select(x => new ODataQueryWrapper(x))
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public Type ElementType => typeof(IODataQueryWrapper);

        public Expression Expression => InnerQuery.Expression;

        public IQueryProvider Provider => new ODataQueryWrapperProvider(InnerQuery.Provider);
    }
}