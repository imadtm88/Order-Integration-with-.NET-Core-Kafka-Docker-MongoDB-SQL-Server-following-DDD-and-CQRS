using Application.Communication;
using Application.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;

namespace Application.Job
{
    //public class FetchOrderJob : IJob
    //{
    //    private readonly RepportHttp _reportHttp;
    //    private readonly ILogger<FetchOrderJob> _logger;

    //    public FetchOrderJob(RepportHttp reportHttp, ILogger<FetchOrderJob> logger)
    //    {
    //        _reportHttp = reportHttp;
    //        _logger = logger;
    //    }

    //    public async Task Execute(IJobExecutionContext context)
    //    {
    //        _logger.LogInformation("FetchOrderJob started");

    //        var orders = await _reportHttp.GetAllOrdersAsync();
    //        if (orders != null && orders.Any())
    //        {
    //            foreach (var order in orders)
    //            {
    //                var fetchedOrder = await _reportHttp.GetOrderAsync(order.Id); // Utilisez Id au lieu de CustomerOrderRef
    //                if (fetchedOrder != null)
    //                {
    //                    var orderDetails = JsonConvert.SerializeObject(fetchedOrder, Formatting.Indented);
    //                    _logger.LogInformation($"Order Details: {orderDetails}");
    //                }
    //                else
    //                {
    //                    _logger.LogWarning($"Order with ID {order.Id} not found."); // Utilisez Id ici aussi
    //                }
    //            }
    //        }
    //        else
    //        {
    //            _logger.LogWarning("No orders found.");
    //        }

    //        _logger.LogInformation("FetchOrderJob completed");
    //    }

    //}

    //public class FetchOrderJob : IJob
    //{
    //    private readonly RepportHttp _reportHttp;
    //    private readonly ExcelReportService _excelReportService;
    //    private readonly ILogger<FetchOrderJob> _logger;
    //    private readonly string _reportPath = @"C:\Users\Imad.ETTAMEN\OneDrive - Akkodis\Bureau\StorageFiles\Repports\OrdersReport.xlsx";

    //    public FetchOrderJob(RepportHttp reportHttp, ExcelReportService excelReportService, ILogger<FetchOrderJob> logger)
    //    {
    //        _reportHttp = reportHttp;
    //        _excelReportService = excelReportService;
    //        _logger = logger;
    //    }

    //    public async Task Execute(IJobExecutionContext context)
    //    {
    //        _logger.LogInformation("FetchOrderJob started");

    //        var orders = await _reportHttp.GetAllOrdersAsync();
    //        if (orders != null && orders.Any())
    //        {
    //            _excelReportService.GenerateExcelReport(orders, _reportPath);
    //            _logger.LogInformation($"Excel report generated at: {_reportPath}");
    //        }
    //        else
    //        {
    //            _logger.LogWarning("No orders found.");
    //        }

    //        _logger.LogInformation("FetchOrderJob completed");
    //    }
    //}

    public class FetchOrderJob : IJob
    {
        private readonly ReportHttp _reportHttp;
        private readonly ExcelReportService _excelReportService;
        private readonly ILogger<FetchOrderJob> _logger;
        private readonly string _reportPath = @"C:\Users\Imad.ETTAMEN\OneDrive - Akkodis\Bureau\StorageFiles\Repports\OrdersReport.xlsx";

        public FetchOrderJob(ReportHttp reportHttp, ExcelReportService excelReportService, ILogger<FetchOrderJob> logger)
        {
            _reportHttp = reportHttp;
            _excelReportService = excelReportService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("FetchOrderJob started");

            try
            {
                var orders = await _reportHttp.GetAllOrdersAsync();
                _logger.LogInformation($"Fetched {orders.Count} orders.");

                if (orders != null && orders.Any())
                {
                    _excelReportService.GenerateExcelReport(orders, _reportPath);
                    _logger.LogInformation($"Excel report generated at: {_reportPath}");
                }
                else
                {
                    _logger.LogWarning("No orders found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in FetchOrderJob: {ex.Message}");
            }

            _logger.LogInformation("FetchOrderJob completed");
        }
    }
}