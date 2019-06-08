﻿using System.Threading;
using System.Threading.Tasks;

namespace Plugin.SettingsFile
{
    public interface ISettingsFile
    {
        //Task<T> GetAsync<T>(CancellationToken cancellationToken) where T : class;
        Task<T> GetConfiguration<T>(CancellationToken cancellationToken) where T : class;
        Task<T> GetConfiguration<T>() where T : class;
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