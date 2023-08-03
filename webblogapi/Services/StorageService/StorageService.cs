using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace webblogapi.Services.FileService
{
    public class StorageService : IStorageService
    {
        private readonly string _connectionString;
        private readonly string _containerName;
        private readonly BlobServiceClient _blobServiceClient;

        public StorageService(IConfiguration configuration)
        {
            _connectionString = configuration["AzureBlobStorage:ConnectionString"];
            _containerName = configuration["AzureBlobStorage:ContainerName"];
            _blobServiceClient = new BlobServiceClient(_connectionString);
        }

        public async Task<string> UploadFileToStorage(Stream fileStream, string fileName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream);
            return blobClient.Uri.ToString();
        }
    }
}
