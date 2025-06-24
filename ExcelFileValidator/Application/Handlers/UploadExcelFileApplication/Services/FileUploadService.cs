using Application.Communication;
using ClosedXML.Excel;
using Domain.Entities;
using Domain.UploadExcelFileDomain.Entities;
using Domain.UploadExcelFileDomain.Repositories;
using Infrastructure.ExcelFileValidationInfra.Interfaces;
using Infrastructure.ExcelToOrderConverterInfra.Interfaces;
using Infrastructure.UploadExcelFileInfra.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Handlers.UploadExcelFileApplication.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IFileStorage _fileStorage;
        private readonly IExcelFileValidator _excelFileValidator;
        private readonly IExcelToOrderConverter _orderConverter;
        private readonly OrderHttpSender _orderHttpSender;
        private List<Order> _orders;

        public FileUploadService(IFileStorage fileStorage, IExcelFileValidator excelFileValidator, IExcelToOrderConverter orderConverter, OrderHttpSender orderHttpSender)
        {
            _fileStorage = fileStorage;
            _excelFileValidator = excelFileValidator;
            _orderConverter = orderConverter;
            _orders = new List<Order>();
            _orderHttpSender = orderHttpSender;
        }

        public async Task UploadFileAsync(IFormFile formFile)
        {
            try
            {
                if (formFile == null || formFile.Length == 0)
                {
                    throw new ArgumentNullException(nameof(formFile), "No file provided.");
                }

                // Types autorisés pour les fichiers Excel
                string[] allowedMimeTypes = new string[]
                {
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "application/vnd.ms-excel"
                };

                if (!Array.Exists(allowedMimeTypes, mimeType => mimeType == formFile.ContentType))
                {
                    throw new InvalidOperationException("Only Excel files (.xlsx, .xls) are allowed.");
                }

                // Vérifier le nom de fichier
                if (!_excelFileValidator.ValidateFileName(formFile.FileName))
                {
                    throw new InvalidOperationException($"Le nom du fichier doit commencer par 'orders_drop'.");
                }

                // Ouvrir le fichier Excel
                using (var workbook = new XLWorkbook(formFile.OpenReadStream()))
                {
                    var worksheet = workbook.Worksheet(1);

                    // Valider les données du fichier Excel
                    var validationErrors = _excelFileValidator.ValidateExcelFile(worksheet);
                    if (validationErrors.Count > 0)
                    {
                        var errorMessage = string.Join("; ", validationErrors);
                        throw new InvalidOperationException($"Validation errors: {errorMessage}");
                    }

                    // Convertir les données de la feuille Excel en objets Order
                    var orders = _orderConverter.Convert(worksheet);

                    // Ajouter les objets Order à la liste interne _orders
                    _orders.AddRange(orders);

                    // Envoi des objets Order à l'API
                    await _orderHttpSender.SendOrdersAsync(orders);
                }
                
                var uploadedFile = new UploadedFile(formFile.FileName, formFile.FileName);

                await _fileStorage.SaveFileAsync(uploadedFile, formFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }
    }
}