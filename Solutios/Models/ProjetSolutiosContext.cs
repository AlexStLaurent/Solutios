using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

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

        public virtual DbSet<Expense> Expense { get; set; }
        public virtual DbSet<FollowUp> FollowUp { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectExpense> ProjectExpense { get; set; }
        public virtual DbSet<ProjectFollowUp> ProjectFollowUp { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseSqlServer("Server= .\\SQLEXPRESS;Database= ProjetSolutios;Trusted_Connection= True;");
                optionsBuilder.UseSqlServer("Data Source=LAPTOP-ROG-DEV;Initial Catalog=ProjetSolutios;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>(entity =>
            {
                entity.Property(e => e.ExpenseId)
                    .HasColumnName("Expense_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.ExpenseDate)
                    .HasColumnName("Expense_Date")
                    .HasColumnType("date");

                entity.Property(e => e.ExpenseInfo).HasColumnName("Expense_Info");
            });

            modelBuilder.Entity<FollowUp>(entity =>
            {
                entity.HasKey(e => e.FuId);

                entity.ToTable("FollowUP");

                entity.Property(e => e.FuId)
                    .HasColumnName("FU_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.FuDate)
                    .HasColumnName("FU_Date")
                    .HasColumnType("date");

                entity.Property(e => e.FuInfo).HasColumnName("FU_Info");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.ProjectId)
                    .HasColumnName("project_id")
                    .UseSqlServerIdentityColumn();

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
            });

            modelBuilder.Entity<ProjectExpense>(entity =>
            {
                entity.HasKey(e => e.PeId);

                entity.ToTable("Project_Expense");

                entity.Property(e => e.PeId)
                    .HasColumnName("PE_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.PeExpenseId).HasColumnName("PE_Expense_id");

                entity.Property(e => e.PeProjectId).HasColumnName("PE_Project_id");

                entity.HasOne(d => d.PeExpense)
                    .WithMany(p => p.ProjectExpense)
                    .HasForeignKey(d => d.PeExpenseId)
                    .HasConstraintName("FK__Project_E__PE_Ex__4316F928");

                entity.HasOne(d => d.PeProject)
                    .WithMany(p => p.ProjectExpense)
                    .HasForeignKey(d => d.PeProjectId)
                    .HasConstraintName("FK__Project_E__PE_Pr__440B1D61");
            });

            modelBuilder.Entity<ProjectFollowUp>(entity =>
            {
                entity.HasKey(e => e.PfId);

                entity.ToTable("Project_FollowUP");

                entity.Property(e => e.PfId)
                    .HasColumnName("PF_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.PfFollowUpId).HasColumnName("PF_FollowUp_id");

                entity.Property(e => e.PfProjectId).HasColumnName("PF_Project_id");

                entity.HasOne(d => d.PfFollowUp)
                    .WithMany(p => p.ProjectFollowUp)
                    .HasForeignKey(d => d.PfFollowUpId)
                    .HasConstraintName("FK__Project_F__PF_Fo__3D5E1FD2");

                entity.HasOne(d => d.PfProject)
                    .WithMany(p => p.ProjectFollowUp)
                    .HasForeignKey(d => d.PfProjectId)
                    .HasConstraintName("FK__Project_F__PF_Pr__3E52440B");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .UseSqlServerIdentityColumn();
                

                entity.Property(e => e.UserAddress)
                    .HasColumnName("user_address")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserAddress2)
                    .HasColumnName("user_address2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserCity)
                    .HasColumnName("user_city")
                    .HasMaxLength(50)
                    .IsUnicode(false);

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

                entity.Property(e => e.UserPhone)
                    .HasColumnName("user_phone")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.UserProjet).HasColumnName("user_projet");

                entity.Property(e => e.UserProvince)
                    .HasColumnName("user_province")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserRole)
                    .HasColumnName("user_role")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserZipcode)
                    .HasColumnName("user_zipcode")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });
        }
    }
}
