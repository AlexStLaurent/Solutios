using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Solutios.Models
{
    public partial class ProjetSolutiosContext : DbContext
    {
        public ProjetSolutiosContext()
        {
        }

        public ProjetSolutiosContext(DbContextOptions<ProjetSolutiosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
              optionsBuilder.UseSqlServer("Server=.;Database=ProjetSolutios;User Id=sa;Password=sql");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.ProjectId)
                    .HasColumnName("project_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.ProjectDebut)
                    .HasColumnName("project_debut")
                    .HasColumnType("date");

                entity.Property(e => e.ProjectFin)
                    .HasColumnName("project_fin")
                    .HasColumnType("date");

                entity.Property(e => e.ProjectName)
                    .HasColumnName("project_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectSoumission).HasColumnName("project_soumission");

                entity.Property(e => e.ProjectStatus).HasColumnName("project_status");

                entity.Property(e => e.ProjectSuivi).HasColumnName("project_suivi");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.UserDetail).HasColumnName("user_detail");

                entity.Property(e => e.UserEmail)
                    .HasColumnName("user_email")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserFirstName)
                    .HasColumnName("user_firstName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserMdp)
                    .HasColumnName("user_mdp")
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserProjet).HasColumnName("user_projet");

                entity.Property(e => e.UserRole)
                    .HasColumnName("user_role")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });
        }
    }
}
