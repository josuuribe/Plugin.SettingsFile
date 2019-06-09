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
    public class SettingsFileImplementation<T> : ISettingsFile<T>
        where T : class
    {
        private IRandomAccessStreamWithContentType inputStream;
        private Stream readingStream;

        public Task<T> LoadAsync(string file = "config.json", CancellationToken cancellationToken = default)
        {
            return ConfigurationManager<T>.LoadAsync(GetStreamAsync(file), cancellationToken);
        }

        public T Get()
        {
            return ConfigurationManager<T>.Get();
        }

        private async Task<Stream> GetStreamAsync(string file)
        {
            ReleaseUnmanagedResources();

            var fileApp = await StorageFile.GetFileFromApplicationUriAsync(new Uri($@"ms-appx:///Assets//{file}"));

            inputStream = await fileApp.OpenReadAsync();
            readingStream = inputStream.AsStreamForRead();

            return readingStream;
        }

        private void ReleaseUnmanagedResources()
        {
            inputStream?.Dispose();
            readingStream?.Dispose();

            inputStream = null;
            readingStream = null;
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
