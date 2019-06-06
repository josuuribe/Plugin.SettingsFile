using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Android.App;

namespace Plugin.SettingsFile
{
    /// <summary>
    /// Interface for SettingsFile
    /// </summary>
    public class SettingsFileImplementation : ISettingsFile
    {
        private const string ConfigurationFilePath = "config.json";

        private readonly Func<Context> _contextProvider;

        private Stream _readingStream;

        public AndroidConfigurationStreamProvider(Func<Context> contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public Task<Stream> GetStreamAsync()
        {
            ReleaseUnmanagedResources();

            AssetManager assets = _contextProvider().Assets;

            _readingStream = assets.Open(ConfigurationFilePath);

            return Task.FromResult(_readingStream);
        }

        private void ReleaseUnmanagedResources()
        {
            _readingStream?.Dispose();
            _readingStream = null;
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~SettingsFileImplementation()
        {
            ReleaseUnmanagedResources();
        }
    }
}
