using QueueShared.Queues;
using Serilog;

public class ImpactTransactionService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Log.Information($"----------- {nameof(ImpactTransactionService)} is starting");
        var queue=new ImpactTransaction(true);
        queue.Run();
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(10000);
        }
        queue.Channel.Close();
        queue.Connection.Close();
        await Task.CompletedTask;
    }
}