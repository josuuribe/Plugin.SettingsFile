using System.Threading;
using System.Threading.Tasks;

namespace Plugin.Maui.SettingsFile
{
    public interface ISettingsFile<T>
         where T : class
    {
        /// <summary>
        /// Load a settings file asynchronusly.
        /// </summary>
        /// <param name="file">File name, by defaut 'config.json'.</param>
        /// <param name="cancellationToken">Cancellation token to be used, by defaut 'config.json'.</param>
        /// <returns>Configuration object.</returns>
        Task<T> LoadAsync(string file = "config.json", CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Returns a configuration object.
        /// </summary>
        /// <returns></returns>
        T Get();
    }
}
