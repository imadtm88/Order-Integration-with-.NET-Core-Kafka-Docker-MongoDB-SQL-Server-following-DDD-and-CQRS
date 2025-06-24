using ClosedXML.Excel;
using Domain.Entities;
using Infrastructure.ExcelToOrderConverterInfra.Interfaces;
using System.Globalization;

namespace Application.Handlers.ExcelToOrderConverterApplication.Services
{
    public class ExcelToOrderConverter : IExcelToOrderConverter
    {
        public List<Order> Convert(IXLWorksheet worksheet)
        {
            List<Order> orders = new List<Order>();

            // Les formats de date possibles dans le fichier Excel
            string[] dateFormats = { "yyyy-MM-dd", "MM/dd/yyyy", "dd/MM/yyyy", "yyyy/MM/dd", "MM-dd-yyyy", "dd-MM-yyyy" };

            // Itérer sur chaque ligne de données à partir de la ligne 2
            for (int rowNumber = 2; rowNumber <= worksheet.LastRowUsed().RowNumber(); rowNumber++)
            {
                var row = worksheet.Row(rowNumber);

                // Lire la valeur de la cellule 'CreationDate'
                var dateCellValue = row.Cell(2).GetValue<string>();

                // Initialiser creationDate avec une valeur par défaut
                DateTime creationDate = default;
                bool validDate = false;

                Console.WriteLine($"Row {rowNumber}, CreationDate: {dateCellValue}");

                // Convertir la valeur de la cellule 'CreationDate' en DateTime en essayant plusieurs formats
                foreach (var format in dateFormats)
                {
                    if (DateTime.TryParseExact(dateCellValue, format, null, DateTimeStyles.None, out creationDate))
                    {
                        validDate = true;
                        break;
                    }
                }

                // Essayer une conversion générique si les formats spécifiques échouent
                if (!validDate && DateTime.TryParse(dateCellValue, out creationDate))
                {
                    validDate = true;
                }

                if (!validDate)
                {
                    throw new FormatException($"Cannot convert value of Dropshipment!B{rowNumber} to System.DateTime.");
                }

                // Créer un nouvel objet Order à partir des données de la ligne
                var order = new Order
                {
                    CustomerOrderRef = row.Cell(1).GetValue<int>(),
                    CreationDate = creationDate,
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

                orders.Add(order);
            }

            return orders;
        }
    }
}
