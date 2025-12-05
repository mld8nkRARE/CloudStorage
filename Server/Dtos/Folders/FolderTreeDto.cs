using Server.Dtos.Files;

namespace Server.Dtos.Folders
{
    public class FolderTreeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<FolderTreeDto> SubFolders { get; set; } = new();
        public List<FileResponseDto> Files { get; set; } = new();

    }
}
