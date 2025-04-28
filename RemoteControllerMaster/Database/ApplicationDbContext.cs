using Microsoft.EntityFrameworkCore;
using RemoteControllerMaster.Database.Models;


namespace RemoteControllerMaster.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<Statistic> Statistics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users", schema: "core");
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
                entity.Property(e => e.Login).HasColumnName("login").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Password).HasColumnName("password").IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Machine>(entity =>
            {
                entity.ToTable("machines", schema: "core");
                entity.HasKey(e => e.MachineId);
                entity.Property(e => e.MachineId).HasColumnName("machine_id").IsRequired();
            });

            modelBuilder.Entity<Statistic>(entity =>
            {
                entity.ToTable("statistics", schema: "analytics");

                entity.HasKey(e => new { e.StatisticId, e.Date });

                entity.Property(e => e.StatisticId)
                    .HasColumnName("statistic_id")
                    .IsRequired();

                entity.Property(e => e.MachineId)
                    .HasColumnName("machine_id")
                    .IsRequired();

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .IsRequired();

                entity.HasOne<Machine>()
                    .WithMany()
                    .HasForeignKey(e => e.MachineId)
                    .HasConstraintName("FK_statistics_machines_machine_id");
            });

        }

    }

}
