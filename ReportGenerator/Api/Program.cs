using Application.Communication;
using Application.Job;
using Application.Jobs;
using Application.Services;
using Quartz.Impl;
using Quartz.Spi;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<ReportHttp>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["OrderIntegratorStatusMongo:BaseUrl"]);
});

// Register ExcelReportService
builder.Services.AddSingleton<ExcelReportService>();

// Add Quartz.NET services
builder.Services.AddSingleton<IJobFactory, SingletonJobFactory>();
builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

// Add our job
builder.Services.AddSingleton<FetchOrderJob>();
builder.Services.AddSingleton(new JobSchedule(
    jobType: typeof(FetchOrderJob),
    cronExpression: "0 0/1 * 1/1 * ? *"));

builder.Services.AddHostedService<QuartzHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();