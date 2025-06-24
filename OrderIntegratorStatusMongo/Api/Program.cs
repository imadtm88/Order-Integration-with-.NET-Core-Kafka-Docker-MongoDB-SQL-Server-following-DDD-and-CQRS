using Application.Command.Services;
using Domain.Repositories;
using Infrastructure.KafkaInfraMessagerie;
using Infrastructure.KafkaInfraMessagerie.Config;
using Infrastructure.Repositories;
using MongoDB.Driver;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews()
    .AddJsonOptions(op => op.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddSingleton<IMongoClient, MongoClient>(s =>
    new MongoClient(builder.Configuration.GetSection("MongoDB")["ConnectionString"]));

builder.Services.Configure<KafkaConfig>(builder.Configuration.GetSection("Kafka"));

builder.Services.AddSingleton<KafkaProducer>();

builder.Services.AddHostedService<KafkaConsumer>();

builder.Services.AddHostedService<KafkaPaymentConsumer>();

builder.Services.AddHostedService<KafkaIntegrationConsumer>();

builder.Services.AddSingleton<IOrderRepository, MongoDBOrderRepository>();

builder.Services.AddScoped<OrderServiceApplication>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();