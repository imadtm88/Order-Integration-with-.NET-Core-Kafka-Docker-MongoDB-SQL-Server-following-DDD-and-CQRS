namespace Domain.UploadExcelFileDomain.Entities
{
    public class UploadedFile
    {
        public UploadedFile(string fileName, string filePath)
        {
            FileName = fileName;
            FilePath = filePath;
            UploadDate = DateTime.UtcNow;
        }

        public string? FileName { get; private set; }
        public string? FilePath { get; private set; }
        public DateTime UploadDate { get; private set; }
    }
}