namespace webblogapi.Services.FileService
{
    public interface IStorageService
    {
        Task<string> UploadFileToStorage(Stream fileStream, string fileName);
    }
}
