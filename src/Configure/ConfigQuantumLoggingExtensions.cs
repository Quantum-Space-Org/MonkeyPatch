using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quantum.Configurator;
using Quantum.MonkeyPatch.Service;

namespace Quantum.MonkeyPatch.Configure
{
    public static class ConfigQuantumMonkeyPatchExtensions
    {
        public static ConfigQuantumMonkeyPatchBuilder ConfigQuantumMonkeyPatch(this QuantumServiceCollection collection)
        {
            return new ConfigQuantumMonkeyPatchBuilder(collection);
        }
    }

    public class ConfigQuantumMonkeyPatchBuilder
    {
        private readonly QuantumServiceCollection _quantumServiceCollection;

        public ConfigQuantumMonkeyPatchBuilder(QuantumServiceCollection collection)
        {
            _quantumServiceCollection = collection;
        }


        public ConfigQuantumMonkeyPatchBuilder AddMonkeyPatchService<T>(T services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            _quantumServiceCollection.Collection.Add(new ServiceDescriptor(typeof(IMonkeyPatchServices), services.GetType(), lifetime));
            return this;
        }

        public ConfigQuantumMonkeyPatchBuilder AddDefaultMonkeyPatchService(ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            _quantumServiceCollection.Collection.Add(new ServiceDescriptor(typeof(IMonkeyPatchServices),
                typeof(MonkeyPatchServices), lifetime));
            return this;
        }

        public ConfigQuantumMonkeyPatchBuilder AddDbContext<T>(ServiceLifetime lifetime)
        where T : QuantumMonkeyPatchDbContext
        {
            _quantumServiceCollection.Collection.Add(new ServiceDescriptor(typeof(QuantumMonkeyPatchDbContext), typeof(T),
                lifetime));
            return this;
        }

        public ConfigQuantumMonkeyPatchBuilder AddDbContextOptions<T>(Func<IServiceProvider, DbContextOptions<T>> options,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
        where T : QuantumMonkeyPatchDbContext
        {
            _quantumServiceCollection.Collection.Add(
                new ServiceDescriptor(typeof(DbContextOptions<T>), options, lifetime));
            return this;
        }


        public QuantumServiceCollection and()
        {
            return _quantumServiceCollection;
        }

    }
}
