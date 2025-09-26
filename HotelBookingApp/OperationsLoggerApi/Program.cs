using OperationsLoggerApi.KafkaOperationsLoggerConsumer;

// Build
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<KafkaOptions>(builder.Configuration.GetSection("Kafka"));
builder.WebHost.UseKestrel().UseUrls("http://localhost:5088");

builder.Services.AddHostedService<OpsLogConsumer>();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
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
app.Lifetime.ApplicationStarted.Register(() =>
    Console.WriteLine("Now listening on: " + string.Join(", ", app.Urls)));