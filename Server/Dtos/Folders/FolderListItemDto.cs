namespace Server.Dtos.Folders
{
    public class FolderListItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
