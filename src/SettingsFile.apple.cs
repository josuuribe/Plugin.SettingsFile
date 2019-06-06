using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Plugin.SettingsFile;

namespace Plugin.SettingsFile
{
    /// <summary>
    /// Interface for SettingsFile
    /// </summary>
    public class SettingsFileImplementation : ISettingsFile
    {
        private const string ConfigurationFilePath = "Assets/config.json";

        private Stream _readingStream;

        public Task<Stream> GetStreamAsync()
        {
            ReleaseUnmanagedResources();
            _readingStream = new FileStream(ConfigurationFilePath, FileMode.Open, FileAccess.Read);

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
