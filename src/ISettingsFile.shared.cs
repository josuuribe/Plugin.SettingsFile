using System.Threading;
using System.Threading.Tasks;

namespace Plugin.SettingsFile
{
    public interface ISettingsFile<T>
         where T : class
    {
        Task<T> LoadAsync(string file = "config.json", CancellationToken cancellationToken = default(CancellationToken));
        T Get();
    }
}
