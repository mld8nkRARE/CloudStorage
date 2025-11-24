using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Server.Controllers
{
    [Route("api/files")]
    [ApiController]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly string _storagePath;

        public FilesController(AppDbContext context, IWebHostEnvironment env)
        
        
      {
            _context = context;
            _storagePath = Path.Combine(env.ContentRootPath, "uploads");
            Directory.CreateDirectory(_storagePath); // создаём папку, если нет
        }

        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);


        // 1. ЗАГРУЗКА ФАЙЛА 
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файл не выбран");

            var fileInfo = new StoredFileInfo
            {
                Id = Guid.NewGuid(),
                FileName = file.FileName,
                ContentType = file.ContentType,
                Size = file.Length,
                UserId = CurrentUserId,
                UploadedAt = DateTime.UtcNow,
                LastUpdateAt = DateTime.UtcNow
            };

            var extension = Path.GetExtension(file.FileName);
            var physicalFileName = $"{fileInfo.Id}{extension}";          
            var fullPath = Path.Combine(_storagePath, physicalFileName);

             
            Directory.CreateDirectory(_storagePath);
            await using (var stream = new FileStream(fullPath, FileMode.Create))
                await file.CopyToAsync(stream);
           

            _context.Files.Add(fileInfo);
            await _context.SaveChangesAsync();

            return Ok(new { fileInfo.Id }); // возвращаем клиенту только Id (Guid)
        }

        // 2. СПИСОК ФАЙЛОВ
        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var files = await _context.Files
                .Where(f => f.UserId == CurrentUserId)
                .Select(f => new
                {
                    f.Id,
                    f.FileName,
                    f.ContentType,
                    f.Size,
                    f.UploadedAt
                })
                .OrderByDescending(f => f.UploadedAt)
                .ToListAsync();

            return Ok(files);
        }

        // 3. СКАЧИВАНИЕ — сюда вставляем код "При скачивании"
        [HttpGet("download/{id:guid}")]
        public async Task<IActionResult> Download(Guid id)
        {
            var fileInfo = await _context.Files
                .FirstOrDefaultAsync(f => f.Id == id && f.UserId == CurrentUserId);

            if (fileInfo == null)
                return NotFound();

            // ← ВОТ ЭТОТ БЛОК — тот самый "при скачивании"
            var extension = Path.GetExtension(fileInfo.FileName);
            var physicalFileName = $"{fileInfo.Id}{extension}";
            var fullPath = Path.Combine(_storagePath, physicalFileName);

            if (!System.IO.File.Exists(fullPath))
                return NotFound();
            // ← конец блока

            return PhysicalFile(fullPath, fileInfo.ContentType, fileInfo.FileName);
        }

        // 4. УДАЛЕНИЕ
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var fileInfo = await _context.Files
                .FirstOrDefaultAsync(f => f.Id == id && f.UserId == CurrentUserId);

            if (fileInfo == null)
                return NotFound();

            var extension = Path.GetExtension(fileInfo.FileName);
            var physicalFileName = $"{fileInfo.Id}{extension}";
            var fullPath = Path.Combine(_storagePath, physicalFileName);

            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);

            _context.Files.Remove(fileInfo);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
