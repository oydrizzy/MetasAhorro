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

            // GOAL
            modelBuilder.Entity<Goal>(entity =>
            {
                entity.Property(g => g.OwnerKey)
                      .HasMaxLength(64)
                      .IsRequired()
                      .HasDefaultValue("");   // para migrar sin valores nulos

                entity.HasIndex(g => g.OwnerKey);

                // (Opcional) refuerza restricciones según tu modelo
                entity.Property(g => g.Title).HasMaxLength(80).IsRequired();
                entity.Property(g => g.Description).HasMaxLength(500);
                // Si usas SQLite no forces "decimal(18,2)"; déjalo por defecto
                // entity.Property(g => g.TargetAmount).HasColumnType("decimal(18,2)");
                // entity.Property(g => g.InitialAmount).HasColumnType("decimal(18,2)");
            });

            // DEPOSIT
            modelBuilder.Entity<Deposit>(entity =>
            {
                // entity.Property(d => d.Amount).HasColumnType("decimal(18,2)");

                entity.HasOne<Goal>()
                      .WithMany(g => g.Deposits)
                      .HasForeignKey(d => d.GoalId)
                      .OnDelete(DeleteBehavior.Cascade); // elimina depósitos al borrar la meta
            });
        }
    }
}
