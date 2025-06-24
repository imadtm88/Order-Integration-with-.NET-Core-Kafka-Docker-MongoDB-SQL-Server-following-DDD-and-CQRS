using System.IO;
using DinkToPdf;
using DinkToPdf.Contracts;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace Application.Services
{
    public class ExcelReportService
    {
        private readonly ILogger<ExcelReportService> _logger;

        public ExcelReportService(ILogger<ExcelReportService> logger)
        {
            _logger = logger;
        }

        public void GenerateExcelReport(List<Order> orders, string filePath)
        {
            _logger.LogInformation("Starting to generate Excel report.");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("OrdersReport");

                    // Add headers
                    worksheet.Cells[1, 1].Value = "CustomerOrderRef";
                    worksheet.Cells[1, 2].Value = "CustomerNumber";
                    worksheet.Cells[1, 3].Value = "OrderStatus";
                    worksheet.Cells[1, 4].Value = "CreationDate";
                    worksheet.Cells[1, 5].Value = "Email";
                    worksheet.Cells[1, 6].Value = "PhoneNumber";
                    worksheet.Cells[1, 7].Value = "ProductId";

                    // Calculate the maximum number of status updates among all orders
                    int maxStatuses = orders.Max(o => o.OrderHistoryStatus.Count);
                    if (maxStatuses > 1)
                    {
                        for (int i = 0; i < maxStatuses; i++)
                        {
                            worksheet.Cells[1, 8 + i * 2].Value = $"Status {i + 1}";
                            worksheet.Cells[1, 9 + i * 2].Value = $"Date Modification {i + 1}";
                        }
                    }

                    // Add rows
                    for (int i = 0; i < orders.Count; i++)
                    {
                        var order = orders[i];
                        worksheet.Cells[i + 2, 1].Value = order.CustomerOrderRef;
                        worksheet.Cells[i + 2, 2].Value = order.CustomerNumber;
                        worksheet.Cells[i + 2, 3].Value = order.OrderStatus;
                        worksheet.Cells[i + 2, 4].Value = order.CreationDate.ToString("yyyy-MM-dd HH:mm:ss");
                        worksheet.Cells[i + 2, 5].Value = order.Email;
                        worksheet.Cells[i + 2, 6].Value = order.PhoneNumber;
                        worksheet.Cells[i + 2, 7].Value = order.ProductId;

                        if (order.OrderHistoryStatus.Count > 1)
                        {
                            for (int j = 0; j < order.OrderHistoryStatus.Count; j++)
                            {
                                worksheet.Cells[i + 2, 8 + j * 2].Value = order.OrderHistoryStatus[j].PreviousStatus;
                                worksheet.Cells[i + 2, 9 + j * 2].Value = order.OrderHistoryStatus[j].LastUpdated.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                        }
                    }

                    // Format the header
                    using (var range = worksheet.Cells[1, 1, 1, 7 + (maxStatuses > 1 ? maxStatuses * 2 : 0)])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    }

                    // AutoFit columns
                    worksheet.Cells.AutoFitColumns();

                    // Save the file
                    var file = new FileInfo(filePath);
                    package.SaveAs(file);

                    _logger.LogInformation($"Excel report successfully generated at: {filePath}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error generating Excel report: {ex.Message}");
            }
        }
    }
}




//{
//    public class ExcelReportService
//    {
//        public void GenerateExcelReport(List<Order> orders, string filePath)
//        {
//            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

//            using (var package = new ExcelPackage())
//            {
//                var worksheet = package.Workbook.Worksheets.Add("OrdersReport");

//                // Add headers
//                worksheet.Cells[1, 1].Value = "CustomerOrderRef";
//                worksheet.Cells[1, 2].Value = "CustomerNumber";
//                worksheet.Cells[1, 3].Value = "OrderStatus";
//                worksheet.Cells[1, 4].Value = "OrderHistoryStatus";
//                worksheet.Cells[1, 5].Value = "CreationDate";
//                worksheet.Cells[1, 6].Value = "Email";
//                worksheet.Cells[1, 7].Value = "PhoneNumber";
//                worksheet.Cells[1, 8].Value = "ProductId";

//                // Add rows
//                for (int i = 0; i < orders.Count; i++)
//                {
//                    var order = orders[i];
//                    worksheet.Cells[i + 2, 1].Value = order.CustomerOrderRef;
//                    worksheet.Cells[i + 2, 2].Value = order.CustomerNumber;
//                    worksheet.Cells[i + 2, 3].Value = order.OrderStatus;
//                    worksheet.Cells[i + 2, 4].Value = string.Join(", ", order.OrderHistoryStatus.Select(s => s.PreviousStatus + " (" + s.LastUpdated.ToString("yyyy-MM-dd HH:mm:ss") + ")"));
//                    worksheet.Cells[i + 2, 5].Value = order.CreationDate.ToString("yyyy-MM-dd HH:mm:ss");
//                    worksheet.Cells[i + 2, 6].Value = order.Email;
//                    worksheet.Cells[i + 2, 7].Value = order.PhoneNumber;
//                    worksheet.Cells[i + 2, 8].Value = order.ProductId;
//                }

//                // Format the header
//                using (var range = worksheet.Cells[1, 1, 1, 8])
//                {
//                    range.Style.Font.Bold = true;
//                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
//                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
//                }

//                // AutoFit columns
//                worksheet.Cells.AutoFitColumns();

//                // Save the file
//                var file = new FileInfo(filePath);
//                package.SaveAs(file);
//            }
//        }
//    }
//}