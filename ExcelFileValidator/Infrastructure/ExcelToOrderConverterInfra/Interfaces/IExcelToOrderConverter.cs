using ClosedXML.Excel;
using Domain.Entities;

namespace Infrastructure.ExcelToOrderConverterInfra.Interfaces
{
    public interface IExcelToOrderConverter
    {
        List<Order> Convert(IXLWorksheet worksheet);
    }
}