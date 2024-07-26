using Microsoft.EntityFrameworkCore;
using MusicTool.Models.Domain;

namespace MusicTool.DataAccess.Data
{
    public class MusicToolDbContext : DbContext
    {
        public MusicToolDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Định nghĩa Revenue như một loại thực thể không có khóa
            modelBuilder.Entity<Models.DTO.Revenue>()
                .HasNoKey()
                .ToView(null);
        }

        public DbSet<Account> Account { get; set; }
        public DbSet<Song> Song { get; set; }
        public DbSet<Singer> Singer { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Song_Category> Song_Category { get; set; }
        public DbSet<Song_Singer> Song_Singer { get; set; }
        public DbSet<VIP> VIP { get; set; }
    }
}
