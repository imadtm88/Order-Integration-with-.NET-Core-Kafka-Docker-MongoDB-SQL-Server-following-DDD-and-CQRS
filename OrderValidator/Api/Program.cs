using Application.Communication;
using Application.Services;
using Infrastructure.Interfaces;
using Infrastructure.KafkaInfraMessagerie;
using Infrastructure.KafkaInfraMessagerie.Config;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));

builder.Services.AddSingleton<IOrderService, OrderHttpService>();

builder.Services.AddSingleton<IOrderValidator, OrderValidator>(serviceProvider =>
{
    var validator = new OrderValidator();
    validator.SetReferencePrice(100);
    return validator;
});

builder.Services.AddSingleton<KafkaProducer>();

builder.Services.AddHostedService<KafkaConsumer>();

builder.Services.AddHttpClient<OrderHttpService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5187");
});
//.AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600)))
//.AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.Run();