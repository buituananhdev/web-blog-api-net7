using Microsoft.AspNetCore.Mvc;
using webblogapi.Services.FileService;

namespace webblogapi.Controllers
{
    [Route("api/storages")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly IStorageService _StorageService;

        public StorageController(IStorageService StorageService)
        {
            _StorageService = StorageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    string fileName = file.FileName;
                    string fileUrl = await _StorageService.UploadFileToStorage(stream, fileName);
                    return Ok(new { FileName = fileName, FileUrl = fileUrl });
                }
            }
            catch (Exception ex)    
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { status = "failure", message = "Loi server" });
            }
        }
    }
}
