using System.Threading;
using System.Threading.Tasks;

namespace Plugin.SettingsFile
{
    public interface ISettingsFile
    {
        Task<T> GetConfigurationAsync<T>(string file = "config.json", CancellationToken cancellationToken = default(CancellationToken)) where T : class;
    }

    /*
    public interface IConfigurationStreamProvider : IDisposable
    {
        Task<Stream> GetStreamAsync();
    }
    

    public interface IConfigurationStreamProviderFactory
    {
        IConfigurationStreamProvider Create();
    }
    */
}
