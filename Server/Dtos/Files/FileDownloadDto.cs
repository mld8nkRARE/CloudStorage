namespace Server.Dtos.Files
{
    public class FileDownloadDto
    {
        public Stream FileStream { get; set; } = default!;
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
    }
}
