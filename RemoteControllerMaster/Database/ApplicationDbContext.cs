using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        public DbSet<User2Permission> User2Permissions { get; set; }
        public DbSet<AuthorizeToken> AuthorizeTokens { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<CommandType> CommandTypes { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<User2Machine> Users2Machines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users", schema: "core");
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
                entity.Property(e => e.Login).HasColumnName("login").IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Machine>(entity =>
            {
                entity.ToTable("machines", schema: "core");
                entity.HasKey(e => e.MachineId);
                entity.Property(e => e.MachineId).HasColumnName("machine_id").IsRequired();
                entity.Property(e => e.MachineName).HasColumnName("machine_name").IsRequired();
                entity.Property(e => e.IpAddress).HasColumnName("ip_address").IsRequired();
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

                entity.Property(e => e.Variable)
                    .HasColumnName("variable")
                    .IsRequired();

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .IsRequired();

                entity.HasOne<Machine>()
                    .WithMany()
                    .HasForeignKey(e => e.MachineId)
                    .HasConstraintName("FK_statistics_machines_machine_id");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("permissions", schema: "enum");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasConversion(
                        new ValueConverter<Enums.Permission, int>(
                            v => (int)v,
                            v => (Enums.Permission)v
                        )
                    ).HasColumnName("id").IsRequired();
                entity.Property(e => e.Name).HasColumnName("name").IsRequired();
            });

            modelBuilder.Entity<User2Permission>(entity =>
            {
                entity.ToTable("users_permissions", schema: "core");
                entity.HasKey(e => new { e.UserId, e.Permission });
                entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
                entity.Property(e => e.Permission).HasColumnName("permission").IsRequired();

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .HasConstraintName("FK_users_permissions_User_id");

                entity.HasOne<Permission>()
                    .WithMany()
                    .HasForeignKey(e => e.Permission)
                    .HasConstraintName("FK_users_permissions_Permission");
            });

            modelBuilder.Entity<AuthorizeToken>(entity =>
            {
                entity.ToTable("authorize_tokens", schema: "core");

                entity.HasKey(e => e.AuthorizeTokenId);

                entity.Property(e => e.AuthorizeTokenId)
                    .HasColumnName("authorize_token_id")
                    .IsRequired();

                entity.Property(e => e.RefreshToken)
                    .HasColumnName("refresh_token")
                    .IsRequired();

                entity.Property(e => e.ExpiryDate)
                    .HasColumnName("expiry_date")
                    .IsRequired();

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .IsRequired();

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .HasConstraintName("FK_authorize_tokens_user_id");
            });

            modelBuilder.Entity<UserLog>(entity =>
            {
                entity.ToTable("user_logs", "analytics");
                entity.HasKey(e => new { e.UserLogId, e.CreatedAt });

                entity.Property(e => e.UserLogId).HasColumnName("user_log_id").IsRequired();
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Request).HasColumnName("request").HasColumnType("text");
                entity.Property(e => e.Response).HasColumnName("response").HasColumnType("text");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId);
            });

            modelBuilder.Entity<CommandType>(entity =>
            {
                entity.ToTable("command_types", schema: "enum");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasConversion(
                        new ValueConverter<Enums.CommandType, int>(
                            v => (int)v,
                            v => (Enums.CommandType)v
                        )
                    ).HasColumnName("id").IsRequired();
                entity.Property(e => e.Name).HasColumnName("name").IsRequired();
            });

            modelBuilder.Entity<Command>(entity =>
            {
                entity.ToTable("commands", "core");
                entity.HasKey(e => e.CommandId);

                entity.Property(e => e.CommandId).HasColumnName("command_id").IsRequired();
                entity.Property(e => e.Name).HasColumnName("name").IsRequired();
                entity.Property(e => e.CommandText).HasColumnName("command_text").HasColumnType("text").IsRequired();
                entity.Property(e => e.AdditionalInformationText).HasColumnName("additional_information_text").HasColumnType("text");
                entity.Property(e => e.CommandType).HasColumnName("command_type").IsRequired();

                entity.HasOne<CommandType>()
                    .WithMany()
                    .HasForeignKey(e => e.CommandType);
            });

            modelBuilder.Entity<User2Machine>(entity =>
            {
                entity.ToTable("users_machines", schema: "core");
                entity.HasKey(e => new { e.UserId, e.MachineId });
                entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
                entity.Property(e => e.MachineId).HasColumnName("machine_id").IsRequired();

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId);

                entity.HasOne<Machine>()
                    .WithMany()
                    .HasForeignKey(e => e.MachineId);
            });

        }

    }

}
