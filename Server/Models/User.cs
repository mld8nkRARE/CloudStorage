using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nickname { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        // Самое важное: переименовать + сделать private set
        [Required]
        public string PasswordHash { get; private set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Навигационное свойство — всё ок
        public List<StoredFileInfo> Files { get; set; } = new();

        // Вспомогательный метод — только сервер может менять хеш
        public void SetPasswordHash(string hash)
        {
            PasswordHash = hash;
        }
    }
}
