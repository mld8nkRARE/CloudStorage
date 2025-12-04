using Server.Dtos.Files;

namespace Server.Dtos.Folders
{
    public class FolderContentDto
    {
        public Guid? FolderId { get; set; }
        public string? FolderName { get; set; }
        public List<FolderListItemDto> Folders { get; set; } = new();
        public List<FileListItemDto> Files { get; set; } = new();
    }
}
