using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Plugin.Maui.SettingsFile
{
    public class SettingsFileImplementation<T> : ISettingsFile<T>
        where T : class
    {
        private Stream readingStream;

        public Task<T> LoadAsync(string file = "config.json", CancellationToken cancellationToken = default)
        {
            return ConfigurationManager<T>.LoadAsync(GetStreamAsync(file), cancellationToken);
        }

        public T Get()
        {
            return ConfigurationManager<T>.Get();
        }

        private Task<Stream> GetStreamAsync(string file)
        {
            ReleaseUnmanagedResources();
            readingStream = new FileStream($@"Assets/{file}", FileMode.Open, FileAccess.Read);

            return Task.FromResult(readingStream);
        }

        private void ReleaseUnmanagedResources()
        {
            readingStream?.Dispose();
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
