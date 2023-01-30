using Microsoft.EntityFrameworkCore;

namespace UniversityMonitoring.Data.Models
{
    public partial class UniversitiesMonitoringContext : DbContext
    {
        public UniversitiesMonitoringContext()
        {
        }

        public UniversitiesMonitoringContext(DbContextOptions<UniversitiesMonitoringContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Moderator> Moderators { get; set; } = null!;
        public virtual DbSet<University> Universities { get; set; } = null!;
        public virtual DbSet<UniversityService> UniversityServices { get; set; } = null!;
        public virtual DbSet<UniversityServiceReport> UniversityServiceReports { get; set; } = null!;
        public virtual DbSet<UniversityServiceStateChange> UniversityServiceStateChanges { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserRateOfService> UserRateOfServices { get; set; } = null!;
        public virtual DbSet<UserSubscribeToService> UserSubscribeToServices { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Moderator>(entity =>
            {
                entity.ToTable("Moderator");

                entity.Property(e => e.PasswordSha256hash)
                    .HasColumnType("tinyblob")
                    .HasColumnName("PasswordSHA256Hash");
            });

            modelBuilder.Entity<University>(entity =>
            {
                entity.ToTable("University");

                entity.HasIndex(e => e.Name, "University_Name_uindex")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .HasMaxLength(128)
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");
            });

            modelBuilder.Entity<UniversityService>(entity =>
            {
                entity.ToTable("UniversityService");

                entity.HasIndex(e => e.UniversityId, "UniversityService_University_Id_fk");

                entity.Property(e => e.IpAddress).HasColumnType("tinyblob");

                entity.Property(e => e.Name)
                    .HasMaxLength(128)
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");

                entity.HasOne(d => d.University)
                    .WithMany(p => p.UniversityServices)
                    .HasForeignKey(d => d.UniversityId)
                    .HasConstraintName("UniversityService_University_Id_fk");
            });

            modelBuilder.Entity<UniversityServiceReport>(entity =>
            {
                entity.ToTable("UniversityServiceReport");

                entity.HasIndex(e => e.ServiceId, "UniversityServiceReport_UniversityService_Id_fk");

                entity.HasIndex(e => e.IssuerId, "UniversityServiceReport_User_Id_fk");

                entity.Property(e => e.Content)
                    .HasMaxLength(4096)
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");

                entity.HasOne(d => d.Issuer)
                    .WithMany(p => p.UniversityServiceReports)
                    .HasForeignKey(d => d.IssuerId)
                    .HasConstraintName("UniversityServiceReport_User_Id_fk");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.UniversityServiceReports)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("UniversityServiceReport_UniversityService_Id_fk");
            });

            modelBuilder.Entity<UniversityServiceStateChange>(entity =>
            {
                entity.ToTable("UniversityServiceStateChange");

                entity.HasIndex(e => e.ServiceId, "UniversityServiceStateChange_UniversityService_Id_fk");

                entity.Property(e => e.ChangedAt)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.UniversityServiceStateChanges)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("UniversityServiceStateChange_UniversityService_Id_fk");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Email, "User_Email_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.TelegramTag, "User_TelegramTag_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.Username, "User_Username_uindex")
                    .IsUnique();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.PasswordSha256hash)
                    .HasColumnType("tinyblob")
                    .HasColumnName("PasswordSHA256Hash");

                entity.Property(e => e.TelegramTag).HasMaxLength(128);

                entity.Property(e => e.Username).HasMaxLength(64);
            });

            modelBuilder.Entity<UserRateOfService>(entity =>
            {
                entity.ToTable("UserRateOfService");

                entity.HasIndex(e => e.ServiceId, "UserRateOfService_UniversityService_Id_fk");

                entity.HasIndex(e => e.AuthorId, "UserRateOfService_User_Id_fk");

                entity.Property(e => e.Comment)
                    .HasMaxLength(4096)
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.UserRateOfServices)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("UserRateOfService_User_Id_fk");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.UserRateOfServices)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("UserRateOfService_UniversityService_Id_fk");
            });

            modelBuilder.Entity<UserSubscribeToService>(entity =>
            {
                entity.ToTable("UserSubscribeToService");

                entity.HasIndex(e => e.ServiceId, "UserSubscribeToService_UniversityService_Id_fk");

                entity.HasIndex(e => e.UserId, "UserSubscribeToService_User_Id_fk");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.UserSubscribeToServices)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("UserSubscribeToService_UniversityService_Id_fk");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSubscribeToServices)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("UserSubscribeToService_User_Id_fk");
            });
        }
    }
}
