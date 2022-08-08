using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Statistics.Infrastructure.Entities;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {

        var connection = @"Server=db;User=sa;Password=pass@Pass1;Database=API_SQL_test;";
      
        services.AddDbContextFactory<StatisticsContext>(options =>
        {
            options.UseSqlServer(connection);
        });
 
    })
    .Build();

await host.RunAsync();
