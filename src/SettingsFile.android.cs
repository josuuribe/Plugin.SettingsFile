using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Android.App;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.Res;
using Plugin.CurrentActivity;
using System.Threading;

namespace Plugin.SettingsFile
{
    /// <summary>
    /// Interface for SettingsFile
    /// </summary>
    public class SettingsFileImplementation : ISettingsFile
    {
        private const string ConfigurationFilePath = "config.json";

        private readonly Context _contextProvider;

        private Stream _readingStream;

        public SettingsFileImplementation()
        {
            _contextProvider = CrossCurrentActivity.Current.AppContext;
        }

        public Task<T> GetConfigurationAsync<T>() where T : class
        {
            return ConfigurationManager.GetAsync<T>(GetStreamAsync());
        }

        public Task<T> GetConfigurationAsync<T>(CancellationToken cancellationToken) where T : class
        {
            return ConfigurationManager.GetAsync<T>(GetStreamAsync(), cancellationToken);
        }

        private Task<Stream> GetStreamAsync()
        {
            ReleaseUnmanagedResources();

            AssetManager assets = _contextProvider.Assets;

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
