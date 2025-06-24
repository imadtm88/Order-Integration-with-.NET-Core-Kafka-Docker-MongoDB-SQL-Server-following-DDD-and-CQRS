using ClosedXML.Excel;

namespace Infrastructure.ExcelFileValidationInfra.Interfaces
{
    public interface IExcelFileValidator
    {
        List<string> ValidateExcelFile(IXLWorksheet worksheet);
        bool ValidateFileName(string fileName);
    }
}