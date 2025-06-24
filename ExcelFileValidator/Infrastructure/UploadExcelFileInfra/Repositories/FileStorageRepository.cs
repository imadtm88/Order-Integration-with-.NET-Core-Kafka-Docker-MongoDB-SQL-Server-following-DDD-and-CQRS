using Domain.UploadExcelFileDomain.Entities;
using Domain.UploadExcelFileDomain.Repositories;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.UploadExcelFileInfra.Repositories
{
    public class FileStorageRepository : IFileStorage
    {
        private readonly string _storagePath;
        public FileStorageRepository(string storagePath)
        {
            _storagePath = storagePath;

            // Créer le dossier de stockage s'il n'existe pas
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }

        // Enregistrer le fichier
        public async Task SaveFileAsync(UploadedFile file, IFormFile formFile)
        {
            // Chemin complet pour enregistrer le fichier
            var filePath = Path.Combine(_storagePath, file.FileName);

            // Vérifiez si le fichier existe déjà
            if (File.Exists(filePath))
            {
                throw new InvalidOperationException($"Le fichier '{file.FileName}' existe déjà dans le chemin de stockage.");
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await formFile.CopyToAsync(fileStream);
            }
        }

        // Obtenir le chemin de stockage
        public string GetStoragePath()
        {
            return _storagePath;
        }

        // Supprimer un fichier
        public async Task DeleteFileAsync(string fileName)
        {
            var fullPath = Path.Combine(_storagePath, fileName);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        // Obtenir un fichier par nom
        public async Task<UploadedFile> GetFileByNameAsync(string fileName)
        {
            var fullPath = Path.Combine(_storagePath, fileName);

            if (!File.Exists(fullPath))
            {
                return null;
            }

            // Chargez les données du fichier
            return new UploadedFile(fileName, fullPath);
        }
    }
}