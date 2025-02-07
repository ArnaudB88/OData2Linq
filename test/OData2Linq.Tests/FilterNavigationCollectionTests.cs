namespace OData2Linq
{
    using System.Collections;
    using System.Linq;

    using OData2Linq.SampleData;

    using Xunit;

    public class FilterNavigationCollectionTests
    {
        [Fact]
        public void WhereCol1()
        {
            var result = ClassWithCollection.CreateQuery().OData().Filter("Link2/any(s: s/Id eq 311)").ToArray();

            Assert.Single((IEnumerable)result);
            Assert.Equal(31, result[0].Id);
        }
    }
}