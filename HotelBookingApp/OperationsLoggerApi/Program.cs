using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using OperationsLoggerApi.Data;
using OperationsLoggerApi.KafkaOperationsLoggerConsumer;

Env.Load();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<KafkaOptions>(builder.Configuration.GetSection("Kafka"));

builder.Services.AddDbContext<OpsLogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHostedService<OpsLogConsumer>();
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
