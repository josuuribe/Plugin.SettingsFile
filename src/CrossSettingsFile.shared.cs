using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Plugin.SettingsFile
{
    /// <summary>
    /// Cross SettingsFile
    /// </summary>
    public static class CrossSettingsFile
    {
        static Lazy<ISettingsFile> implementation = new Lazy<ISettingsFile>(() => CreateSettingsFile(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Gets if the plugin is supported on the current platform.
        /// </summary>
        public static bool IsSupported => implementation.Value == null ? false : true;

        /// <summary>
        /// Current plugin implementation to use
        /// </summary>
        public static ISettingsFile Current
        {
            get
            {
                ISettingsFile ret = implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static ISettingsFile CreateSettingsFile()
        {
#if NETSTANDARD1_0 || NETSTANDARD2_0
            return null;
#else
#pragma warning disable IDE0022 // Use expression body for methods
            ISettingsFile settings = new SettingsFileImplementation();
            ConfigurationManager.Initialize(settings);
            return settings;
#pragma warning restore IDE0022 // Use expression body for methods
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly() =>
            new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");


    }

    internal sealed class ConfigurationManager
    {
        private readonly SemaphoreSlim _semaphoreSlim;

        //private static IConfigurationStreamProviderFactory _factory;

        private static ISettingsFile settingsFile = null;

        private bool _initialized;
        //private Configuration _configuration;

        protected ConfigurationManager()
        {
            _semaphoreSlim = new SemaphoreSlim(1, 1);
        }

        private static ConfigurationManager Instance { get; } = new ConfigurationManager();

        /*
        public static void Initialize(IConfigurationStreamProviderFactory factory)
        {
            _factory = factory;
        }
        */

        public static void Initialize(ISettingsFile settings)
        {
            settingsFile = settings;
        }

        public async Task<T> GetAsync<T>(CancellationToken cancellationToken)
            where T : class
        {
            var configuration = await InitializeAsync<T>(cancellationToken).ConfigureAwait(false);

            if (configuration == null)
                throw new InvalidOperationException("Configuration should not be null");

            return configuration;
        }

        private async Task<T> InitializeAsync<T>(CancellationToken cancellationToken) where T : class
        {
            if (_initialized)
                return null;

            try
            {
                await _semaphoreSlim.WaitAsync(cancellationToken).ConfigureAwait(false);

                if (_initialized)
                    return null;

                var configuration = await ReadAsync<T>().ConfigureAwait(false);
                _initialized = true;
                //_configuration = configuration;
                return configuration;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        private async Task<T> ReadAsync<T>() where T : class
        {
            //using (var streamProvider = _factory.Create())
            using (var stream = await settingsFile.GetStreamAsync().ConfigureAwait(false))
            {
                var configuration = Deserialize<T>(stream);
                return configuration;
            }
        }

        private T Deserialize<T>(Stream stream)
        {
            if (stream == null || !stream.CanRead)
                return default(T);

            using (var sr = new StreamReader(stream))
            using (var jtr = new Newtonsoft.Json.JsonTextReader(sr))
            {
                var js = new Newtonsoft.Json.JsonSerializer();
                var value = js.Deserialize<T>(jtr);
                return value;
            }
        }
    }

}
