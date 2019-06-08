using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Plugin.SettingsFile;
using Windows.Storage;
using System.Threading;

namespace Plugin.SettingsFile
{
    /// <summary>
    /// Interface for SettingsFile
    /// </summary>
    public class SettingsFileImplementation : ISettingsFile
    {
        private IRandomAccessStreamWithContentType _inputStream;
        private Stream _readingStream;

        private const string ConfigurationFilePath = "ms-appx:///Assets/config.json";

        public Task<T> GetConfiguration<T>(CancellationToken cancellationToken) where T : class
        {
            return ConfigurationManager.GetAsync<T>(GetStreamAsync(), cancellationToken);
        }

        public Task<T> GetConfiguration<T>() where T : class
        {
            return ConfigurationManager.GetAsync<T>(GetStreamAsync());
        }

        private async Task<Stream> GetStreamAsync()
        {
            ReleaseUnmanagedResources();

            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(ConfigurationFilePath));

            _inputStream = await file.OpenReadAsync();
            _readingStream = _inputStream.AsStreamForRead();

            return _readingStream;
        }

        private void ReleaseUnmanagedResources()
        {
            _inputStream?.Dispose();
            _readingStream?.Dispose();

            _inputStream = null;
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
