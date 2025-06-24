using ClosedXML.Excel;
using Domain.Entities;
using Infrastructure.ExcelFileValidationInfra.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Application.Handlers.ExcelFileValidationApplication.Services
{
    public class ExcelFileValidator : IExcelFileValidator
    {
        private readonly IConfiguration _configuration;
        private readonly string _filePrefix;
        private readonly string _sheetName;

        public ExcelFileValidator(IConfiguration configuration)
        {
            _configuration = configuration;
            _filePrefix = _configuration["ExcelFileValidation:FilePrefix"];
            _sheetName = _configuration["ExcelFileValidation:SheetName"];
        }

        public bool ValidateFileName(string fileName)
        {
            return fileName.StartsWith(_filePrefix);
        }

        public List<string> ValidateExcelFile(IXLWorksheet worksheet)
        {
            var validationErrors = new List<string>();

            // Vérifier que la feuille de calcul est nommée correctement
            if (!worksheet.Name.Equals(_sheetName, StringComparison.OrdinalIgnoreCase))
            {
                validationErrors.Add($"La feuille de calcul doit être nommée '{_sheetName}'.");
                return validationErrors;
            }

            // Colonnes attendues
            string[] expectedColumns = new string[]
            {
                "CustomerOrderRef",
                "CreationDate",
                "CustomersReference",
                "CustomerNumber",
                "LastName",
                "FirstName",
                "ShippingAddress1",
                "ShippingZipCode",
                "ShippingCity",
                "ShippingCountry",
                "PhoneNumber",
                "Email",
                "ProductId",
                "PriceWithoutTax",
                "Quantity",
                "SellerID",
                "OfferID"
            };

            // Vérification des colonnes attendues
            var headerRow = worksheet.Row(1);
            if (headerRow == null || headerRow.IsEmpty())
            {
                validationErrors.Add("Le fichier Excel est vide ou ne contient pas les colonnes requises (ligne d'en-tête manquante).");
                return validationErrors;
            }

            foreach (var expectedColumn in expectedColumns)
            {
                bool columnExists = false;
                foreach (var cell in headerRow.Cells())
                {
                    if (cell.Value.ToString().Equals(expectedColumn, StringComparison.OrdinalIgnoreCase))
                    {
                        columnExists = true;
                        break;
                    }
                }
                if (!columnExists)
                {
                    validationErrors.Add($"La colonne '{expectedColumn}' est manquante.");
                }
            }

            // Validation des types de données
            for (int rowNumber = 2; rowNumber <= worksheet.LastRowUsed().RowNumber(); rowNumber++)
            {
                var row = worksheet.Row(rowNumber);
                try
                {
                    // Créez une instance d'`ExcelRow` à partir de la ligne actuelle
                    var excelRow = new ExcelRow
                    {
                        CustomerOrderRef = row.Cell(1).GetValue<int>(),
                        CreationDate = DateTime.Parse(row.Cell(2).GetString()),
                        CustomersReference = row.Cell(3).GetValue<string>(),
                        CustomerNumber = row.Cell(4).GetValue<string>(),
                        LastName = row.Cell(5).GetValue<string>(),
                        FirstName = row.Cell(6).GetValue<string>(),
                        ShippingAddress1 = row.Cell(7).GetValue<string>(),
                        ShippingZipCode = row.Cell(8).GetValue<int>(),
                        ShippingCity = row.Cell(9).GetValue<string>(),
                        ShippingCountry = row.Cell(10).GetValue<string>(),
                        PhoneNumber = row.Cell(11).GetValue<string>(),
                        Email = row.Cell(12).GetValue<string>(),
                        ProductId = row.Cell(13).GetValue<string>(),
                        PriceWithoutTax = row.Cell(14).GetValue<double>(),
                        Quantity = row.Cell(15).GetValue<int>(),
                        SellerID = row.Cell(16).GetValue<int>(),
                        OfferID = row.Cell(17).GetValue<string>()
                    };
                }
                catch (Exception ex)
                {
                    validationErrors.Add($"Erreur à la ligne {rowNumber}: {ex.Message}");
                }
            }

            return validationErrors;
        }
    }
}