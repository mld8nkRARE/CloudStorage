namespace Server.Dtos.Files
{
    public class FileListItemDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long Size { get; set; }
        public DateTime UploadedAt { get; set; }
        public DateTime LastUpdateAt { get; set; }
    }
}
