namespace OData2Linq.Settings
{
    using Microsoft.OData.UriParser;
    using System;

    public record ODataSettings
    {
        private static readonly object SyncObj = new object();

        private static Action<ODataSettings>? Initializer;

        /// <summary>
        /// Sets the action which will be used to initialize every instance of <type ref="ODataSettings"></type>.
        /// </summary>
        /// <param name="initializer">The action which will be used to initialize every instance of <type ref="ODataSettings"></type>.</param>
        /// <exception cref="ArgumentNullException">initializer</exception>
        /// <exception cref="InvalidOperationException">SetInitializer</exception>
        public static void SetInitializer(Action<ODataSettings> initializer)
        {
            if (initializer == null)
            {
                throw new ArgumentNullException(nameof(initializer));
            }

            lock (SyncObj)
            {
                if (Initializer == null)
                {
                    Initializer = initializer;

                    return;
                }
            }

            throw new InvalidOperationException($"{nameof(SetInitializer)} method can be invoked only once");
        }

        public ODataQuerySettings QuerySettings { get; } = new ODataQuerySettings { PageSize = 20 };

        public ODataValidationSettings ValidationSettings { get; } = new ODataValidationSettings();

        public ODataUriParserSettings ParserSettings { get; } = new ODataUriParserSettings();

        public ODataUriResolver Resolver { get; set; } = new StringAsEnumResolver { EnableCaseInsensitive = true };

        public bool EnableCaseInsensitive
        {
            get => Resolver.EnableCaseInsensitive;
            set => Resolver.EnableCaseInsensitive = value;
        }

        public DefaultQueryConfigurations DefaultQueryConfigurations { get; } = new DefaultQueryConfigurations
        {
            EnableFilter = true,
            EnableOrderBy = true,
            EnableExpand = true,
            EnableSelect = true,
            MaxTop = 100
        };

        public ODataSettings()
        {
            Initializer?.Invoke(this);

            HashCode.Combine(new AbandonedMutexException());
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();

            hash.Add(DefaultQueryConfigurations.EnableExpand);
            hash.Add(DefaultQueryConfigurations.EnableSelect);
            hash.Add(DefaultQueryConfigurations.EnableCount);
            hash.Add(DefaultQueryConfigurations.EnableOrderBy);
            hash.Add(DefaultQueryConfigurations.EnableFilter);
            hash.Add(DefaultQueryConfigurations.MaxTop);
            hash.Add(DefaultQueryConfigurations.EnableSkipToken);

            hash.Add(QuerySettings.HandleNullPropagation);
            hash.Add(QuerySettings.PageSize);
            hash.Add(QuerySettings.ModelBoundPageSize);
            hash.Add(QuerySettings.EnsureStableOrdering);
            hash.Add(QuerySettings.EnableConstantParameterization);
            hash.Add(QuerySettings.TimeZone);
            hash.Add(QuerySettings.EnableCorrelatedSubqueryBuffering);
            hash.Add(QuerySettings.IgnoredQueryOptions);
            hash.Add(QuerySettings.IgnoredNestedQueryOptions);
            hash.Add(QuerySettings.HandleReferenceNavigationPropertyExpandFilter);

            hash.Add(ParserSettings.MaximumExpansionCount);
            hash.Add(ParserSettings.MaximumExpansionDepth);

            return hash.ToHashCode();
        }
    }
}