using Server.Models;

namespace Server.Services
{
    public interface IFileService
    {
        /// <summary>
        /// Загружает файл и возвращает его модель
        /// </summary>
        Task<FileModel> UploadAsync(IFormFile file, Guid userId, Guid? folderId = null);

        /// <summary>
        /// Возвращает список файлов пользователя
        /// </summary>
        Task<IEnumerable<FileModel>> GetFilesAsync(Guid userId, Guid? folderId = null);

        /// <summary>
        /// Возвращает информацию о файле для скачивания (проверяет права)
        /// </summary>
        Task<FileModel?> GetFileInfoForDownloadAsync(Guid fileId, Guid userId);

        /// <summary>
        /// Удаляет файл (проверяет права)
        /// </summary>  
        Task<bool> DeleteAsync(Guid fileId, Guid userId);

        /// <summary>
        /// Возвращает общий размер файлов пользователя
        /// </summary>
        Task<long> GetUserUsedBytesAsync(Guid userId);
       
    }
}
