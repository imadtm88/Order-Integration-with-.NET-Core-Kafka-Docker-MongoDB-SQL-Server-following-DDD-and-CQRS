using Application.Communication;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.KafkaInfraMessagerie;
using Infrastructure.KafkaInfraMessagerie.Config;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.Configure<KafkaConfig>(builder.Configuration.GetSection("Kafka"));
builder.Services.AddHostedService<KafkaConsumer>();
builder.Services.AddSingleton<KafkaProducer>();

builder.Services.AddHttpClient<IOrderIntegrationHttp, OrderIntegrationHttp>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["OrderIntegratorStatusMongo:BaseUrl"]);
});

builder.Services.AddDbContext<OrderIntegratorDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
