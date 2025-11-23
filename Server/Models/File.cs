using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class StoredFileInfo
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(255)]
        public string FileName { get; set; } = string.Empty;     // оригинальное имя (от пользователя)

        [Required]
        public string ContentType { get; set; } = string.Empty;  // image/jpeg, application/pdf и т.д.

        [Required]
        public long Size { get; set; }                           // размер в байтах

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public DateTime LastUpdateAt { get; set; } = DateTime.UtcNow;

        // Внешний ключ
        [Required]
        public int UserId { get; set; }

        // Навигационное свойство
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        // Удобный метод — возвращает имя файла на диске (Id + расширение)
        public string GetPhysicalFileName()
        {
            var extension = Path.GetExtension(FileName);
            return $"{Id}{extension}";
        }
    }
}
