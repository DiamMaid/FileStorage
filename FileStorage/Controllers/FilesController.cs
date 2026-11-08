using System.Security.Claims;
using FileStorage.Application.DTOs;
using FileStorage.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileStorage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [Authorize]
        [RequestSizeLimit(50 * 1024 * 1024)]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { error = "Файл не выбран" });

            using var stream = file.OpenReadStream();
            var result = await _fileService.UploadAsync(
                file.FileName,
                file.ContentType,
                file.Length,
                GetUserId(),
                stream);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(Guid id)
        {
            var result = await _fileService.DownloadAsync(id, GetUserId());
            if (result == null)
                return NotFound(new { error = "Файл не найден" });

            return File(result.Value.Stream, result.Value.ContentType, result.Value.FileName);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetFiles()
        {
            var files = await _fileService.GetFilesAsync(GetUserId());
            return Ok(files);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _fileService.DeleteAsync(id, GetUserId());
            if (!deleted)
                return NotFound(new { error = "Файл не найден" });

            return NoContent();
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }
    }
}