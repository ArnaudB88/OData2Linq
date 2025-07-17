namespace OData2Linq.Settings
{
    public class ODataRawQueryOptions
    {
        public string? Filter { get; set; }

        public string? Apply { get; set; }

        public string? Compute { get; set; }

        public string? Search { get; set; }

        public string? OrderBy { get; set; }

        public string? Top { get; set; }

        public string? Skip { get; set; }

        public string? Select { get; set; }

        public string? Expand { get; set; }

        public string? Count { get; set; }

        public string? Format { get; set; }

        public string? SkipToken { get; set; }

        public string? DeltaToken { get; set; }
    }
}