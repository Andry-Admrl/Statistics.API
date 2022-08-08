using Microsoft.EntityFrameworkCore;
using Statistics.Infrastructure.Entities;
using Statistics.Queue.Interfaces;
using Statistics.Queue.Models;
using System.Data;

namespace Statistics.Queue
{
    public class Worker : BackgroundService
    {
        private readonly IDbContextFactory<StatisticsContext> _contextFactory;
        private readonly IMessageProducer _messagePublisher;
        private readonly ILogger<Worker> _logger;
        private readonly CancellationTokenSource _cts = new();

        public Worker(IDbContextFactory<StatisticsContext> contextFactory, IMessageProducer messagePublisher, ILogger<Worker> logger)
        {
            _contextFactory = contextFactory;
            _messagePublisher = messagePublisher;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var _timer = new PeriodicTimer(TimeSpan.FromSeconds(10));

            try
            {
                while (await _timer.WaitForNextTickAsync(_cts.Token) && !stoppingToken.IsCancellationRequested)
                {
                    await DoWorkAsync(stoppingToken);
                }
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError(ex, "Operation canceled exception");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Queue writer service exception.");
            }
        }

        private async Task DoWorkAsync(CancellationToken stoppingToken)
        {
           using (var context = _contextFactory.CreateDbContext())
            {
                 using (var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead))
                {
                    var totalCount = await context.Calls.SumAsync(c => c.Count);

                    var from = DateTime.UtcNow.AddDays(-2);
                    var to = DateTime.UtcNow;

                    var averageCallsInHour = await context.Calls
                        .Where(c => c.CreatedAt > from && c.CreatedAt <= to)
                        .SumAsync(x => x.Count)/48;

                    var countOfObservedDay = context.Calls.GroupBy(c => c.CreatedAt.Date).Count();
                    var mostLoadedHour = await context.Calls
                           .GroupBy(c => c.CreatedAt.Hour)
                           .Select(g => new
                           {
                               Hour = g.Key,
                               CountCalls = g.Sum(c => c.Count / countOfObservedDay)
                           })
                           .OrderByDescending(v => v.CountCalls)
                           .FirstAsync();

                    _messagePublisher.SendProductMessage(
                        new Statics()
                        {
                            TotalCountOfCalls = totalCount,
                            AverageCountOfCalls = averageCallsInHour,
                            MostLoadedHour = mostLoadedHour.Hour
                        });
                    Console.WriteLine("sending to rabbit queue");
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }
    }
}
