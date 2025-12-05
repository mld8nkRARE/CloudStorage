namespace Server.Dtos.Folders
{
    public class FolderDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid? ParentFolderId { get; set; }
    }
}
