namespace OData2Linq.Wrappers
{
    using System.Linq.Expressions;
    using Microsoft.AspNetCore.OData.Query.Wrapper;

    internal class ODataQueryWrapperProvider : IQueryProvider
    {
        private IQueryProvider InnerProvider { get; }

        public ODataQueryWrapperProvider(IQueryProvider innerProvider)
        {
            InnerProvider = innerProvider;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new ODataQueryWrapperQueryable(InnerProvider.CreateQuery<ISelectExpandWrapper>(expression));
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            if (typeof(TElement) != typeof(IODataQueryWrapper))
            {
                return InnerProvider.CreateQuery<TElement>(expression);
            }

            return (IQueryable<TElement>)new ODataQueryWrapperQueryable(InnerProvider.CreateQuery<ISelectExpandWrapper>(expression));
        }

        public object? Execute(Expression expression)
        {
            return InnerProvider.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return InnerProvider.Execute<TResult>(expression);
        }
    }
}