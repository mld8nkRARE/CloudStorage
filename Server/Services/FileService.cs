using Server.Data;
using Server.Dtos.Files;
using Server.Models;
using Microsoft.EntityFrameworkCore;


namespace Server.Services
{
    public class FileService //: IFileService
    {
        private readonly AppDbContext _context;
        private readonly string _storagePath;

        public FileService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _storagePath = Path.Combine(env.ContentRootPath, "uploads");
            Directory.CreateDirectory(_storagePath);
        }

        public async Task<Guid> UploadAsync(IFormFile file, int userId)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Файл не выбран");

            var fileInfo = new FileModel
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                Size = file.Length,
                UserId = userId,
                UploadedAt = DateTime.UtcNow,
                LastUpdateAt = DateTime.UtcNow
            };

            // Id генерируется автоматически EF Core (Guid.NewGuid())

            var fullPath = Path.Combine(_storagePath, fileInfo.GetPhysicalFileName());

            await using (var stream = new FileStream(fullPath, FileMode.Create))
                await file.CopyToAsync(stream);

            _context.Files.Add(fileInfo);
            await _context.SaveChangesAsync();

            return fileInfo.Id;
        }

        public async Task<IEnumerable<FileListItemDto>> GetListAsync(int userId)
        {
            return await _context.Files
                .Where(f => f.UserId == userId)
                .Select(f => new FileListItemDto
                {
                    Id = f.Id,
                    FileName = f.FileName,
                    ContentType = f.ContentType,
                    Size = f.Size,
                    UploadedAt = f.UploadedAt
                })
                .OrderByDescending(f => f.UploadedAt)
                .ToListAsync();
        }

        public async Task<FileModel?> GetFileInfoForDownloadAsync(Guid fileId, int userId)
        {
            var fileInfo = await _context.Files
                .FirstOrDefaultAsync(f => f.Id == fileId && f.UserId == userId);

            if (fileInfo == null)
                return null;

            var fullPath = Path.Combine(_storagePath, fileInfo.GetPhysicalFileName());

            if (!System.IO.File.Exists(fullPath))
                return null;

            return new FileModel
            {
                FilePath = fullPath,
                FileName = fileInfo.FileName,
                ContentType = fileInfo.ContentType
            };
        }

        public async Task<bool> DeleteAsync(Guid fileId, int userId)
        {
            var fileInfo = await _context.Files
                .FirstOrDefaultAsync(f => f.Id == fileId && f.UserId == userId);

            if (fileInfo == null)
                return false;

            var fullPath = Path.Combine(_storagePath, fileInfo.GetPhysicalFileName());

            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);

            _context.Files.Remove(fileInfo);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
