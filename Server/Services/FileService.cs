using Server.Data;
using AutoMapper;
using Server.Dtos.Files;
using Server.Models;
using Microsoft.EntityFrameworkCore;


namespace Server.Services
{
    public class FileService : IFileService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly string _storagePath;

        public FileService(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _storagePath = Path.Combine(env.ContentRootPath, "uploads");
            Directory.CreateDirectory(_storagePath);
        }

        public async Task<FileModel> UploadAsync(IFormFile file, Guid userId, Guid? folderId = null)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Файл не выбран");
            if (folderId.HasValue)
            {
                var folder = await _db.Folders
                    .FirstOrDefaultAsync(f => f.Id == folderId.Value && f.UserId == userId);

                if (folder == null)
                    throw new UnauthorizedAccessException("Папка не найдена или доступ запрещён");
            }
            var fileInfo = new FileModel
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                Size = file.Length,
                UserId = userId,
            };

            // Id генерируется автоматически EF Core (Guid.NewGuid())

            var fullPath = Path.Combine(_storagePath, fileInfo.GetPhysicalFileName());

            await using (var stream = new FileStream(fullPath, FileMode.Create))
                await file.CopyToAsync(stream);

            _db.Files.Add(fileInfo);
            await _db.SaveChangesAsync();

            return fileInfo;
        }

        public async Task<IEnumerable<FileModel>> GetListFilesAsync(Guid userId, Guid? folderId = null)
        {
            var query = _db.Files
                .Where(f => f.UserId == userId);

            if (folderId.HasValue)
                query = query.Where(f => f.FolderId == folderId);
            else
                query = query.Where(f => f.FolderId == null); // файлы в корне

            return await query
                .OrderByDescending(f => f.UploadedAt)
                .Select(f => new FileModel
                {
                    Id = f.Id,
                    FileName = f.FileName,
                    ContentType = f.ContentType,
                    Size = f.Size,
                    UploadedAt = f.UploadedAt,
                    FolderId = f.FolderId
                })
                .ToListAsync();
        }

        public async Task<FileDownloadDto?> GetFileInfoForDownloadAsync(Guid fileId, Guid userId)
        {
            var fileInfo = await _db.Files
                .FirstOrDefaultAsync(f => f.Id == fileId && f.UserId == userId);

            if (fileInfo == null)
                return null;

            var fullPath = Path.Combine(_storagePath, fileInfo.GetPhysicalFileName());

            if (!System.IO.File.Exists(fullPath))
                return null;

            return new FileDownloadDto
            {
                FilePath = fullPath,
                FileName = fileInfo.FileName,
                ContentType = fileInfo.ContentType
            };
        }

        public async Task<bool> DeleteAsync(Guid fileId, Guid userId)
        {
            var fileInfo = await _db.Files
                .FirstOrDefaultAsync(f => f.Id == fileId && f.UserId == userId);

            if (fileInfo == null)
                return false;

            var fullPath = Path.Combine(_storagePath, fileInfo.GetPhysicalFileName());

            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);

            _db.Files.Remove(fileInfo);
            await _db.SaveChangesAsync();

            return true;
        }
        public async Task<long> GetUserUsedBytesAsync(Guid userId)
        {
            return await _db.Files
                .Where(f => f.UserId == userId)
                .SumAsync(f => f.Size);
        }
    }
}
