using Microsoft.AspNetCore.Http;

namespace Infrastructure.UploadExcelFileInfra.Interfaces
{
    public interface IFileUploadService
    {
        Task UploadFileAsync(IFormFile formFile);
    }
}