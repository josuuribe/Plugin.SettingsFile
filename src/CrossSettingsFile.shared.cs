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
        private static Lazy<ISettingsFile> implementation = new Lazy<ISettingsFile>(() => CreateSettingsFile(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

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

        private static ISettingsFile CreateSettingsFile()
        {
#if NETSTANDARD1_0 || NETSTANDARD2_0
            return null;
#else
#pragma warning disable IDE0022 // Use expression body for methods
            return new SettingsFileImplementation();
#pragma warning restore IDE0022 // Use expression body for methods
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly() =>
            new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");


    }

    internal static class ConfigurationManager
    {
        private static readonly SemaphoreSlim _semaphoreSlim;

        //private static IConfigurationStreamProviderFactory _factory;

        private static ISettingsFile settingsFile = null;

        private static bool _initialized;
        //private Configuration _configuration;

        static ConfigurationManager()
        {
            _semaphoreSlim = new SemaphoreSlim(1, 1);
        }

        //private static ConfigurationManager Instance { get; } = new ConfigurationManager();

        /*
        public static void Initialize(IConfigurationStreamProviderFactory factory)
        {
            _factory = factory;
        }
        */

        //public static void Initialize(ISettingsFile settings)
        //{
        //    settingsFile = settings;
        //}

        public static async Task<T> GetAsync<T>(Task<Stream> stream, CancellationToken cancellationToken)
    where T : class
        {
            var configuration = await DeserializeAsync<T>(stream, cancellationToken).ConfigureAwait(false);

            if (configuration == null)
                throw new InvalidOperationException("Configuration should not be null");

            return configuration;
        }

        public static async Task<T> GetAsync<T>(Task<Stream> stream)
            where T : class
        {
            return await DeserializeAsync<T>(stream, CancellationToken.None).ConfigureAwait(false);
        }

        private static async Task<T> DeserializeAsync<T>(Task<Stream> streamAsync, CancellationToken cancellationToken) where T : class
        {
            if (_initialized)
                return null;

            try
            {
                await _semaphoreSlim.WaitAsync(cancellationToken).ConfigureAwait(false);

                if (_initialized)
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

                //var configuration = await ReadAsync<T>().ConfigureAwait(false);
                _initialized = true;
                //_configuration = configuration;
                return configuration;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        //private static async Task<T> ReadAsync<T>() where T : class
        //{
        //    //using (var streamProvider = _factory.Create())
        //    using (var stream = await settingsFile.GetStreamAsync().ConfigureAwait(false))
        //    {
        //        var configuration = Deserialize<T>(stream);
        //        return configuration;
        //    }
        //}

        //private static T Deserialize<T>(Stream stream)
        //{
        //    if (stream == null || !stream.CanRead)
        //        return default(T);

        //    using (var sr = new StreamReader(stream))
        //    using (var jtr = new Newtonsoft.Json.JsonTextReader(sr))
        //    {
        //        var js = new Newtonsoft.Json.JsonSerializer();
        //        var value = js.Deserialize<T>(jtr);
        //        return value;
        //    }
        //}
    }

}
