using Application.Communication;
using Application.Handlers.ExcelFileValidationApplication.Services;
using Application.Handlers.ExcelToOrderConverterApplication.Services;
using Application.Handlers.UploadExcelFileApplication.Services;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Domain.UploadExcelFileDomain.Repositories;
using Infrastructure.ExcelFileValidationInfra.Interfaces;
using Infrastructure.ExcelToOrderConverterInfra.Interfaces;
using Infrastructure.UploadExcelFileInfra.Interfaces;
using Infrastructure.UploadExcelFileInfra.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuration de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var configuration = builder.Configuration;

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    // Resolve conflicts between actions
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

    // Other Swagger configurations
    // ...
});

var storagePath = configuration["FileStorage:PathToSaveFiles"];

builder.Services.AddSingleton<IFileStorage>(new FileStorageRepository(storagePath));

builder.Services.AddScoped<IExcelFileValidator, ExcelFileValidator>();

builder.Services.AddScoped<IExcelToOrderConverter, ExcelToOrderConverter>();

builder.Services.AddScoped<IFileUploadService, FileUploadService>();

// HTTP Config :
builder.Services.AddHttpClient<OrderHttpSender>(c =>
{
    c.BaseAddress = new Uri("http://localhost:5187");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();