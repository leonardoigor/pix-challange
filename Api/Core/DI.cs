using Api.Infra.Conntexts;
using Api.Repository.Base;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Api.Core
{
    public static class DI
    {
        public static IServiceCollection DependencyInjection(this IServiceCollection service, ConfigurationManager Configuration)
        {
            Log.Information("Dependencies starteds");
            var env = Configuration.GetSection("Env");
            var conString = env.GetValue<string>("ConnectionString");
            Log.Information(conString);
            service.AddDbContext<PixContext>(op =>
            {

                op.UseSqlServer(conString);
            });
            service.AddSingleton<IServiceCollection>(service);
            //  service.AddScoped<DbContext, PixContext>();
            service.AddTransient(typeof(BaseRepository<,>));

            return service;
        }
    }
}
