namespace OData2Linq
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using Xunit;

    public class JsonTests
    {
        [Fact]
        public void SerializeSelectExpand()
        {
            JToken token = null;// ClassWithCollection.CreateQuery().OData().SelectExpand("Name", "Link2($filter=Id eq 311;$select=Name)").ToJson();
            Assert.NotNull(token);

            Assert.DoesNotContain("ModelID", token.ToString(Formatting.None));
        }

        [Fact]
        public void SerializeSelectExpand2()
        {
            JToken token = null;// ClassWithCollection.CreateQuery().OData().SelectExpand("Name", "Link2($filter=Id eq 311;$select=Name)").ToJson();
            Assert.NotNull(token);

            Assert.DoesNotContain("ModelID", token.ToString(Formatting.None));
        }
    }
}