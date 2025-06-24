using Application.Communication;
using Infrastructure.Interfaces;
using Infrastructure.KafkaInfraMessagerie;
using Infrastructure.KafkaInfraMessagerie.Config;

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
builder.Services.AddHttpClient<IOrderPaymentHttp, OrderPaymentHttpClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["OrderIntegratorStatusMongo:BaseUrl"]);
});
builder.Services.AddTransient<IOrderPaymentValidator, OrderPaymentValidator>();

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