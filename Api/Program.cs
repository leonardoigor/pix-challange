using Api.Core;
using Serilog;

internal class Program
{
    private static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()  // Log to the console
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)  // Log to a file
            .CreateLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.DependencyInjection(builder.Configuration);
            builder.Services.Configure<Env>(builder.Configuration.GetSection("Env"));
            builder.Services.AddHostedService<ImpactTransactionService>();

            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog();
            });
            var app = builder.Build();
            //Database.SetInitializer(new CreateDatabaseIfNotExists<DbContext>());


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
        catch (Exception e)
        {
            Log.Error(e, "An unhandled exception occurred.");
            throw;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
