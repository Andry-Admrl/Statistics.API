using Microsoft.EntityFrameworkCore;
using Statistics.API.Interfaces;
using Statistics.Infrastructure.Entities;
using System.Data;

namespace Statistics.API.Services
{
    public class StatisticsService : BackgroundService
    {
        private readonly IDbContextFactory<StatisticsContext> _contextFactory;
        private readonly ICallService _callService;
        private readonly ILogger<StatisticsService> _logger;
        private readonly CancellationTokenSource _cts = new();
        private DateTime _startDateTime = DateTime.UtcNow;

        public StatisticsService(IDbContextFactory<StatisticsContext> contextFactory, ICallService callService, ILogger<StatisticsService> logger)
        {
            _contextFactory = contextFactory;
            _callService = callService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await DoWorkAsync(stoppingToken);
        }

        private async Task DoWorkAsync(CancellationToken stoppingToken)
        {

            using var _timer = new PeriodicTimer(TimeSpan.FromSeconds(5));
            try
            {
                while (await _timer.WaitForNextTickAsync(_cts.Token) && !stoppingToken.IsCancellationRequested)

                     using (var context = _contextFactory.CreateDbContext())
                    {
                         using (var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead))
                        {
                            var countCallPerInterval = _callService.GetCountAndResetCalls();
                            var endDateTime = DateTime.UtcNow;
                            await context.Calls.AddAsync(new CallInfo(countCallPerInterval, _startDateTime, endDateTime));
                            await context.SaveChangesAsync();
                            _startDateTime = endDateTime;
                            transaction.Commit();
                        }
                    }
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError(ex, "Db writer service exception.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Db writer service exception.");
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }
    }
}
