using BenchmarkDotNet.Attributes;
using OData2Linq.Tests.SampleData;

namespace OData2Linq.Benchmark
{
    public class QueryOperations
    {
        private readonly IQueryable<SimpleClass> _query;

        public QueryOperations()
        {
            _query = SimpleClass.CreateQuery();
        }

        [Benchmark]
        public SimpleClass[] ODataFilter()
        {
            return _query.OData().Filter("Id eq 1").ToArray();
        }

        [Benchmark]
        public SimpleClass ODataOrderByIdDefault()
        {
            return _query.OData().OrderBy("Id,Name").First();
        }
    }
}
