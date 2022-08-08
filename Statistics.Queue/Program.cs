using Microsoft.EntityFrameworkCore;
using Statistics.Infrastructure.Entities;
using Statistics.Queue;
using Statistics.Queue.Interfaces;
using Statistics.Queue.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {

        services.AddDbContextFactory<StatisticsContext>(options =>
        {
                options.UseSqlServer(@"Server=db;User=sa;Password=pass@Pass1;Database=API_SQL_test;");
        });

        services.AddSingleton<IMessageProducer, RabitMQProducer>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
