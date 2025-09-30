namespace OData2Linq
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OData.Edm;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class ODataQuery<T> : IQueryable<T>
    {
        private readonly IQueryable _inner;

        internal ODataQuery(IQueryable inner, IServiceProvider serviceProvider)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            ServiceProvider = serviceProvider;
        }

        public IEdmModel EdmModel => ServiceProvider.GetRequiredService<IEdmModel>();

        public Type ElementType => _inner.ElementType;

        public Expression Expression => _inner.Expression;

        public IQueryProvider Provider => _inner.Provider;

        public IServiceProvider ServiceProvider { get; }

        public IEnumerator GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return (IEnumerator<T>)_inner.GetEnumerator();
        }

        public IQueryable<T> ToOriginalQuery()
        {
            return (IQueryable<T>)_inner;
        }
    }
}
