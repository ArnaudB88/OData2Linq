namespace OData2Linq.Tests.SampleData
{
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract]
    public class SimpleClassDataContract
    {
        private static readonly SimpleClassDataContract[] items =
        [
            new SimpleClassDataContract {Id = 1, Name = "n1", NameToIgnore = "ign1", NameNotMarked = "nm1"},
            new SimpleClassDataContract {Id = 2, Name = "n2", NameToIgnore = "ign2", NameNotMarked = "nm2"}
        ];

        public static IQueryable<SimpleClassDataContract> CreateQuery()
        {
            return items.AsQueryable();
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember(Name = "nameChanged")]
        public required string Name { get; set; }

        [IgnoreDataMember]
        public required string NameToIgnore { get; set; }

        public required string NameNotMarked { get; set; }
    }
}
