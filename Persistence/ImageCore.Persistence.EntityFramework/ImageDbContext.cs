using Microsoft.EntityFrameworkCore;

namespace ImageCore.Persistence.EntityFramework
{
    public class ImageDbContext : DbContext
    {
        public virtual DbSet<Image> Images { get; set; }

        public ImageDbContext(DbContextOptions<ImageDbContext> options) : base(options)
        {

        }

        #region Overrides of DbContext

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Image>()
                .HasIndex(x => x.Key).IsUnique();
        }

        #endregion
    }
}