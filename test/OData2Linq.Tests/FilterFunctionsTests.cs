

namespace OData2Linq
{
    using OData2Linq.SampleData;
    using Xunit;
    using System.Linq;

    public class FilterFunctionsTests
    {
        [Fact]
        public void Contains()
        {
            var result = SimpleClass.CreateQuery().OData().Filter("contains(Name,'1')").ToArray();

            Assert.Single(result);
            Assert.Equal("n1", result[0].Name);
        }

        [Fact]
        public void Substring()
        {
            var result = SimpleClass.CreateQuery().OData().Filter("substring(Name, 1) eq '2'").ToArray();

            Assert.Single(result);
            Assert.Equal("n2", result[0].Name);
        }
    }
}