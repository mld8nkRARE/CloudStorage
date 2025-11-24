using Microsoft.EntityFrameworkCore;
using Server.Models;
namespace Server.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<StoredFileInfo> Files => Set<StoredFileInfo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Уникальные индексы (без NOCASE — просто и работает)
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Nickname)
                .IsUnique();

            // StoredFileInfo: первичный ключ по Guid
            modelBuilder.Entity<StoredFileInfo>(entity =>
            {
                entity.Property(e => e.Id)
                      .ValueGeneratedNever(); // ← ВАЖНО! Говорим EF: не трогай Id, он уже есть
            });

            // Связь один-ко-многим: User → Files (каскадное удаление)
            modelBuilder.Entity<StoredFileInfo>()
                .HasOne(f => f.User)
                .WithMany(u => u.Files)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Индекс по UserId для быстрых запросов списка файлов
            modelBuilder.Entity<StoredFileInfo>()
                .HasIndex(f => f.UserId);
        }
    }
}
