using Domain.UploadExcelFileDomain.Entities;
using Microsoft.AspNetCore.Http;

namespace Domain.UploadExcelFileDomain.Repositories
{
    public interface IFileStorage
    {
        Task SaveFileAsync(UploadedFile file, IFormFile formFile);
        Task DeleteFileAsync(string fileName);
        Task<UploadedFile> GetFileByNameAsync(string fileName);
        string GetStoragePath();
    }
}