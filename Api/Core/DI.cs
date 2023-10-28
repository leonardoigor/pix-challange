using Api.Infra.Conntexts;
using Api.Repository.Base;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Data.Entity;

namespace Api.Core
{
    public static class DI
    {
        public static IServiceCollection DependencyInjection(this IServiceCollection service)
        {
            Log.Information("Dependencies starteds");
            service.AddSingleton<IServiceCollection>(service);
            service.AddScoped<DbContext, DbContext1>();
            service.AddTransient(typeof(BaseRepository<,>));

            return service;
        }
    }
}
