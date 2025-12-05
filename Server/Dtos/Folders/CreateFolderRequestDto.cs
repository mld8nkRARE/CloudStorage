using System.ComponentModel.DataAnnotations;

namespace Server.Dtos.Folders
{
    public class CreateFolderRequestDto
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        public Guid? ParentFolderId { get; set; }
    }
}
