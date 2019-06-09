using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Plugin.SettingsFile
{
    /// <summary>
    /// Cross SettingsFile
    /// </summary>
    public static class CrossSettingsFile<T>
        where T : class
    {
        private static Lazy<ISettingsFile<T>> implementation = new Lazy<ISettingsFile<T>>(() => CreateSettingsFile(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Gets if the plugin is supported on the current platform.
        /// </summary>
        public static bool IsSupported => implementation.Value == null ? false : true;

        /// <summary>
        /// Current plugin implementation to use.
        /// </summary>
        public static ISettingsFile<T> Current
        {
            get
            {
                return implementation.Value == null ? throw NotImplementedInReferenceAssembly() : implementation.Value;
            }
        }

        private static ISettingsFile<T> CreateSettingsFile()
        {
#if NETSTANDARD1_0 || NETSTANDARD2_0
            return null;
#else
#pragma warning disable IDE0022 // Use expression body for methods
            return new SettingsFileImplementation<T>();
#pragma warning restore IDE0022 // Use expression body for methods
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly() =>
            new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }

    internal static class ConfigurationManager<T>
        where T : class
    {
        private static readonly SemaphoreSlim semaphoreSlim;

        private static readonly ISettingsFile<T> settingsFile = null;

        private static bool initialized;

        private static T configuration = default(T);

        static ConfigurationManager()
        {
            semaphoreSlim = new SemaphoreSlim(1, 1);
        }

        internal static async Task<T> LoadAsync(Task<Stream> stream, CancellationToken cancellationToken)
        {
            configuration = await DeserializeAsync(stream, cancellationToken).ConfigureAwait(false);

            return configuration == null ? throw new InvalidOperationException("Configuration should not be null.") : configuration;
        }

        internal static T Get()
        {
            return configuration == null ? throw new InvalidCastException("Configuration not loaded.") : configuration;
        }

        private static async Task<T> DeserializeAsync(Task<Stream> streamAsync, CancellationToken cancellationToken)
        {
            if (initialized)
                return null;

            try
            {
                await semaphoreSlim.WaitAsync(cancellationToken).ConfigureAwait(false);

                if (initialized)
                    return null;

                var stream = await streamAsync.ConfigureAwait(false);

                if (stream == null || !stream.CanRead)
                    return default(T);

                T configuration = null;

                using (var sr = new StreamReader(stream))
                {
                    using (var jtr = new Newtonsoft.Json.JsonTextReader(sr))
                    {
                        var js = new Newtonsoft.Json.JsonSerializer();
                        configuration = js.Deserialize<T>(jtr);
                    }
                }

                initialized = true;
                return configuration;
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}
