using Microsoft.EntityFrameworkCore;
using MetasAhorro.Models;

namespace MetasAhorro.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Goal> Goals => Set<Goal>();
        public DbSet<Deposit> Deposits => Set<Deposit>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Goal>(entity =>
            {
                entity.Property(g => g.OwnerKey)
                      .HasMaxLength(64)
                      .IsRequired()
                      .HasDefaultValue("");   

                entity.HasIndex(g => g.OwnerKey);

                entity.Property(g => g.Title).HasMaxLength(80).IsRequired();
                entity.Property(g => g.Description).HasMaxLength(500);
                // entity.Property(g => g.TargetAmount).HasColumnType("decimal(18,2)");
                // entity.Property(g => g.InitialAmount).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Deposit>(entity =>
            {

                entity.HasOne<Goal>()
                      .WithMany(g => g.Deposits)
                      .HasForeignKey(d => d.GoalId)
                      .OnDelete(DeleteBehavior.Cascade); 
            });
        }
    }
}
