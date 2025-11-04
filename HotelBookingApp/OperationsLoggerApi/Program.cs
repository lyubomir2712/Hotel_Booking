using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using OperationsLoggerApi.Data;
using OperationsLoggerApi.Data.SeedOfWork.SeedWork;
using OperationsLoggerApi.Infrastructure;
using OperationsLoggerApi.Infrastructure.KafkaOperationsLoggerConsumer;
using OperationsLoggerApi.Interfaces;
using OperationsLoggerApi.KafkaOperationsLoggerConsumer;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;

Env.Load();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

//Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
    options.InstanceName = "Operation_"; 
});

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse(
        builder.Configuration["Redis:ConnectionString"], 
        true
    );
    return ConnectionMultiplexer.Connect(configuration);
});

//Database
builder.Services.AddDbContext<OpsLogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Kafka
builder.Services.Configure<KafkaOptions>(builder.Configuration.GetSection("Kafka"));
builder.Services.AddHostedService<OpsLogConsumer>();

//OperationsLog
builder.Services.AddScoped<IAddOperationLogToDbService, AddOperationLogToDbService>();
builder.Services.AddScoped<IAddOperationLogToRedis, AddOperationLogToRedis>();


// AutoMapper
builder.Services.AddAutoMapper(cfg => { }, typeof(Program).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "OpsLog Consumer is running")
   .WithName("Root")
   .WithOpenApi();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
   .WithName("Health")
   .WithOpenApi();

app.Run();
