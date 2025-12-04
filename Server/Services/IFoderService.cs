using Server.Dtos.Folders;
namespace Server.Services
{
    public interface IFoderService
    {
        /// <summary>
        /// Создаёт новую папку
        /// </summary>
        Task<Guid> CreateFolderAsync(string name, Guid userId, Guid? parentFolderId = null);

        /// <summary>
        /// Получает список папок и файлов в указанной папке (или в корне, если folderId = null)
        /// </summary>
        Task<FolderContentDto> GetFolderContentsAsync(Guid? folderId, Guid userId);

        /// <summary>
        /// Удаляет папку и всё её содержимое (вложенные папки и файлы)
        /// </summary>
        Task<bool> DeleteFolderAsync(Guid folderId, Guid userId);

        /// <summary>
        /// Перемещает файл в другую папку
        /// </summary>
        Task<bool> MoveFileToFolderAsync(Guid fileId, Guid? targetFolderId, Guid userId);

        /// <summary>
        /// Перемещает папку в другую папку
        /// </summary>
        Task<bool> MoveFolderToFolderAsync(Guid folderId, Guid? targetFolderId, Guid userId);
    }
}
