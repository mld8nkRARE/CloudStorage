namespace Server.Models
{
    public class File
    {
        public string Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string StoredFileName { get; set; } = string.Empty; // имя файла на сервере
        public string ContentType { get; set; } = string.Empty; 
        public long Size { get; set; } // в байтах
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdateAt { get; set; } = DateTime.UtcNow;

        //внешний ключ
        public int UserId { get; set; }
        public User User { get; set; } = null!; //навигационное свойство
    }
}
