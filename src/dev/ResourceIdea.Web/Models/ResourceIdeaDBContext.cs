using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ResourceIdea.Models
{
    public partial class ResourceIdeaDBContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        private readonly IConfiguration _configuration;
        public ResourceIdeaDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ResourceIdeaDBContext(DbContextOptions<ResourceIdeaDBContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<EngagementTask> EngagementTasks { get; set; } = null!;
        public virtual DbSet<JobPosition> JobPositions { get; set; } = null!;
        public virtual DbSet<TaskAssignment> TaskAssignments { get; set; } = null!;
        public virtual DbSet<JobSkill> JobSkills { get; set; } = null!;
        public virtual DbSet<License> Licenses { get; set; } = null!;
        public virtual DbSet<LicenseType> LicenseTypes { get; set; } = null!;
        public virtual DbSet<LineOfService> LineOfServices { get; set; } = null!;
        public virtual DbSet<LineOfServiceJob> LineOfServiceJobs { get; set; } = null!;
        public virtual DbSet<MigrationHistory> MigrationHistories { get; set; } = null!;
        public virtual DbSet<Engagement> Engagements { get; set; } = null!;
        public virtual DbSet<Resource> Resources { get; set; } = null!;
        public virtual DbSet<ResourceSkill> ResourceSkills { get; set; } = null!;
        public virtual DbSet<RoleGrouping> RoleGroupings { get; set; } = null!;
        public virtual DbSet<RoleGroupingMember> RoleGroupingMembers { get; set; } = null!;
        public virtual DbSet<Skill> Skills { get; set; } = null!;

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     if (!optionsBuilder.IsConfigured)
        //     {
        //         var connectionString = SqlServerConfiguration.GetConnectionString();
        //         ArgumentNullException.ThrowIfNull(connectionString);
        //         optionsBuilder.UseSqlServer(connectionString!);
        //     }
        // }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<string>>(entity => entity.HasNoKey());
            modelBuilder.Entity<IdentityUserRole<string>>(entity => entity.HasKey(x => new {x.UserId, x.RoleId}));
            modelBuilder.Entity<IdentityUserToken<string>>(entity => entity.HasNoKey());
            
            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client");

                entity.Property(e => e.ClientId)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Address)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Industry)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.CompanyCodeNavigation)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(d => d.CompanyCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Client_Company");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.CompanyCode);

                entity.ToTable("Company");

                entity.Property(e => e.CompanyCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationName)
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EngagementTask>(entity =>
            {
                entity.ToTable("Job");

                entity.Property(e => e.Id)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Color)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Manager)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Partner)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.EngagementId)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Engagement)
                    .WithMany(p => p.Jobs)
                    .HasForeignKey(d => d.EngagementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Job_Project");
            });

            modelBuilder.Entity<JobPosition>(entity =>
            {
                entity.ToTable("JobPosition");

                entity.Property(e => e.CompanyCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PositionTitle)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.CompanyCodeNavigation)
                    .WithMany(p => p.JobPositions)
                    .HasForeignKey(d => d.CompanyCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JobPosition_CompanyCode");
            });

            modelBuilder.Entity<TaskAssignment>(entity =>
            {
                entity.ToTable("JobResource");

                entity.Property(e => e.Details)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EndDateTime).HasColumnType("datetime");

                entity.Property(e => e.TaskId)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.ResourceId)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.StartDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskAssignments)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JobResource_Job");

                entity.HasOne(d => d.Resource)
                    .WithMany(p => p.JobResources)
                    .HasForeignKey(d => d.ResourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JobResource_Resource");
            });

            modelBuilder.Entity<JobSkill>(entity =>
            {
                entity.ToTable("JobSkill");

                entity.Property(e => e.JobId)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.SkillId)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.JobSkills)
                    .HasForeignKey(d => d.JobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JobSkill_Job");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.JobSkills)
                    .HasForeignKey(d => d.SkillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JobSkill_Skill");
            });

            modelBuilder.Entity<License>(entity =>
            {
                entity.HasKey(e => e.LicenseKey);

                entity.ToTable("License");

                entity.Property(e => e.LicenseKey)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.CompanyCodeNavigation)
                    .WithMany(p => p.Licenses)
                    .HasForeignKey(d => d.CompanyCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_License_Company");

                entity.HasOne(d => d.LicenseType)
                    .WithMany(p => p.Licenses)
                    .HasForeignKey(d => d.LicenseTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_License_Type");
            });

            modelBuilder.Entity<LicenseType>(entity =>
            {
                entity.ToTable("LicenseType");

                entity.Property(e => e.Fee).HasColumnType("money");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Plan)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<LineOfService>(entity =>
            {
                entity.ToTable("LineOfService");

                entity.HasIndex(e => e.CompanyCode, "IX_CompanyCode");

                entity.Property(e => e.LineOfServiceId).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CompanyCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.CompanyCodeNavigation)
                    .WithMany(p => p.LineOfServices)
                    .HasForeignKey(d => d.CompanyCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.LineOfService_dbo.Company_CompanyCode");
            });

            modelBuilder.Entity<LineOfServiceJob>(entity =>
            {
                entity.ToTable("LineOfServiceJob");

                entity.HasIndex(e => e.JobId, "IX_JobId");

                entity.HasIndex(e => e.LineOfServiceId, "IX_LineOfServiceId");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.JobId)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.LineOfServiceJobs)
                    .HasForeignKey(d => d.JobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.LineOfServiceJob_dbo.Job_JobId");

                entity.HasOne(d => d.LineOfService)
                    .WithMany(p => p.LineOfServiceJobs)
                    .HasForeignKey(d => d.LineOfServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.LineOfServiceJob_dbo.LineOfService_LineOfServiceId");
            });

            modelBuilder.Entity<MigrationHistory>(entity =>
            {
                entity.HasKey(e => new { e.MigrationId, e.ContextKey })
                    .HasName("PK_dbo.__MigrationHistory");

                entity.ToTable("__MigrationHistory");

                entity.Property(e => e.MigrationId).HasMaxLength(150);

                entity.Property(e => e.ContextKey).HasMaxLength(300);

                entity.Property(e => e.ProductVersion).HasMaxLength(32);
            });

            modelBuilder.Entity<Engagement>(entity =>
            {
                entity.ToTable("Project");

                entity.Property(e => e.EngagementId)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.ClientId)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Color)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Project_Client");
            });

            modelBuilder.Entity<Resource>(entity =>
            {
                entity.ToTable("Resource");

                entity.HasIndex(e => e.Email, "UX_ResourceEmail")
                    .IsUnique();

                entity.Property(e => e.ResourceId)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Fullname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.JobsManagedColor)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.JoinDate).HasColumnType("date");

                entity.Property(e => e.TerminationDate).HasColumnType("date");

                entity.HasOne(d => d.CompanyCodeNavigation)
                    .WithMany(p => p.Resources)
                    .HasForeignKey(d => d.CompanyCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Resource_Company");

                entity.HasOne(d => d.JobPosition)
                    .WithMany(p => p.Resources)
                    .HasForeignKey(d => d.JobPositionId)
                    .HasConstraintName("FK_Resource_JobPosition");
            });

            modelBuilder.Entity<ResourceSkill>(entity =>
            {
                entity.ToTable("ResourceSkill");

                entity.Property(e => e.ResourceId)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.SkillId)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.HasOne(d => d.Resource)
                    .WithMany(p => p.ResourceSkills)
                    .HasForeignKey(d => d.ResourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResourceSkill_Resource");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.ResourceSkills)
                    .HasForeignKey(d => d.SkillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResourceSkill_Skill");
            });

            modelBuilder.Entity<RoleGrouping>(entity =>
            {
                entity.ToTable("RoleGrouping");

                entity.HasIndex(e => e.CompanyCode, "IX_CompanyCode");

                entity.Property(e => e.RoleGroupingId).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CompanyCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.CompanyCodeNavigation)
                    .WithMany(p => p.RoleGroupings)
                    .HasForeignKey(d => d.CompanyCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.RoleGrouping_dbo.Company_CompanyCode");
            });

            modelBuilder.Entity<RoleGroupingMember>(entity =>
            {
                entity.ToTable("RoleGroupingMember");

                entity.HasIndex(e => e.RoleGroupingId, "IX_RoleGroupingId");

                entity.Property(e => e.RoleGroupingMemberId).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.RoleId).HasMaxLength(128);

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.RoleGrouping)
                    .WithMany(p => p.RoleGroupingMembers)
                    .HasForeignKey(d => d.RoleGroupingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.RoleGroupingMember_dbo.RoleGrouping_RoleGroupingId");
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.ToTable("Skill");

                entity.Property(e => e.SkillId)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
