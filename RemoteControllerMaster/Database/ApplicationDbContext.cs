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
                entity.ToTable("Users", schema: "core");
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Login).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Machine>(entity =>
            {
                entity.ToTable("Machines", schema: "core");
                entity.HasKey(e => e.MachineId);
                entity.Property(e => e.MachineId).IsRequired();
            });

            modelBuilder.Entity<Statistic>(entity =>
            {
                entity.ToTable("Statistics", schema: "analytics");
                entity.HasKey(e => e.StatisticId);
                entity.Property(e => e.StatisticId).IsRequired();
                entity.Property(e => e.Date).IsRequired();
            });
        }

    }

}
