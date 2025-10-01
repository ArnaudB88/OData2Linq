namespace OData2Linq.Tests
{
    using Microsoft.OData;
    using Wrappers;
    using SampleData;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class SelectTests
    {
        [Fact]
        public void SelectDefault()
        {
            IODataQueryWrapper[] result = SimpleClass.CreateQuery().OData().SelectExpand().ToArray();

            IDictionary<string, object?> metadata = result[0].ToDictionary();

            Assert.Equal(SimpleClass.NumberOfProperties, metadata.Count);
        }

        [Fact]
        public void SelectAllt()
        {
            IODataQueryWrapper[] result = SimpleClass.CreateQuery().OData().SelectExpand("*").ToArray();

            IDictionary<string, object?> metadata = result[0].ToDictionary();

            Assert.Equal(SimpleClass.NumberOfProperties, metadata.Count);
        }

        [Fact]
        public void SelectName()
        {
            IODataQueryWrapper[] result = SimpleClass.CreateQuery().OData().SelectExpand("Name").ToArray();

            IDictionary<string, object?> metadata = result[0].ToDictionary();

            // Expect Name to be selected
            Assert.Single(metadata);
            Assert.Equal("Name", metadata.Single().Key);
            Assert.Equal("n1", metadata.Single().Value);
            Assert.IsType<string>(metadata.Single().Value);
        }

        [Fact]
        public void SelectDataMember()
        {
            IODataQueryWrapper[] result = SimpleClassDataContract.CreateQuery().OData().SelectExpand("nameChanged").ToArray();

            IDictionary<string, object?> metadata = result[0].ToDictionary();

            // Expect Name to be selected
            Assert.Single(metadata);
            Assert.Equal("nameChanged", metadata.Single().Key);
            Assert.Equal("n1", metadata.Single().Value);
            Assert.IsType<string>(metadata.Single().Value);
        }

        [Fact]
        public void SelectId()
        {
            IODataQueryWrapper[] result = SimpleClass.CreateQuery().OData().SelectExpand("Id").ToArray();

            IDictionary<string, object?> metadata = result[0].ToDictionary();

            Assert.Single(metadata);
            Assert.Equal("Id", metadata.Single().Key);
            Assert.IsType<int>(metadata.Single().Value);
            Assert.Equal(1, metadata.Single().Value);
        }

        [Fact]
        public void SelectIdCaseInsensitive()
        {
            IODataQueryWrapper[] result = SimpleClass.CreateQuery().OData().SelectExpand("id").ToArray();

            IDictionary<string, object?> metadata = result[0].ToDictionary();

            Assert.Single(metadata);
            Assert.Equal("Id", metadata.Single().Key, StringComparer.Ordinal);
            Assert.IsType<int>(metadata.Single().Value);
            Assert.Equal(1, metadata.Single().Value);
        }

        [Fact]
        public void SelectCaseSensitiveOnDemand()
        {
            Assert.Throws<ODataException>(() => SimpleClass.CreateQuery().OData(s => s.EnableCaseInsensitive = false).SelectExpand("id"));
        }

        [Fact]
        public void SelectNotExistingProperty()
        {
            Assert.Throws<ODataException>(() => SimpleClass.CreateQuery().OData().SelectExpand("asdcaefacfawrcfwrfaw4"));
        }

        [Fact]
        public void SelectNameToIgnore()
        {
            Assert.Throws<ODataException>(() => SimpleClass.CreateQuery().OData().SelectExpand("NameToIgnore"));
        }

        [Fact]
        public void SelectDisabled()
        {
            IODataQueryWrapper[] result = ClassWithLink.CreateQuery().OData().SelectExpand("Link4").ToArray();

            IDictionary<string, object?> metadata = result[0].ToDictionary();

            Assert.Empty(metadata);
        }

        [Fact]
        public void SelectLinkWithoutExpandNotWorking()
        {
            IODataQueryWrapper[] result = ClassWithLink.CreateQuery().OData().SelectExpand("Link1").ToArray();

            IDictionary<string, object?> metadata = result[0].ToDictionary();

            Assert.Empty(metadata);
        }
    }
}