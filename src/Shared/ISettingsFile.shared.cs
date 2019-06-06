using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Plugin.SettingsFile
{
    public interface ISettingsFile
    {
        //Task<T> GetAsync<T>(CancellationToken cancellationToken) where T : class;
        Task<Stream> GetStreamAsync();
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
