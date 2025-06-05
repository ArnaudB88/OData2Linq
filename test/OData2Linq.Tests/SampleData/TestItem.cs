namespace OData2Linq.Tests.SampleData
{
    /// <summary>
    /// A utility class for use in ODataTests
    /// </summary>
    public class TestItem
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public int Number { get; set; }
    }

}