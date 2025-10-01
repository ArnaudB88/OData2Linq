namespace OData2Linq.DependencyInjection
{
    internal static class ServicesExtensions
    {
        public static T GetRequiredService<T>(this IServiceProvider provider) where T : notnull
        {
            object? service = provider.GetService(typeof(T));

            if (service == null)
            {
                throw new InvalidOperationException($"No service of type {typeof(T).Name} is registered.");
            }

            return (T)service;
        }
    }
}