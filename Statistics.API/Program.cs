using Microsoft.EntityFrameworkCore;
using Statistics.API.Interfaces;
using Statistics.API.Middleware;
using Statistics.API.Services;
using Statistics.Infrastructure.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddDbContextFactory<StatisticsContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"));
});

builder.Services.AddSingleton<ICallService, CallService>();
builder.Services.AddHostedService<StatisticsService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

PrepDB.Prepopulation(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<CallCounterMiddleware>();
app.MapControllers();

app.Run();
