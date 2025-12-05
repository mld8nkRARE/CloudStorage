namespace Server.Dtos.Files
{
    public class FileResponseDto
    {
        public Guid Id;
        public string FileName;
        public string ContentType;
        public long Size;
        public DateTime UploadedAt;
        public DateTime LastUpdateAt;
        public Guid? FolderId; // null = корень
    }
}
