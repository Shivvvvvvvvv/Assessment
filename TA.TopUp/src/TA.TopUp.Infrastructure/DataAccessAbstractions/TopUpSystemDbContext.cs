using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TA.TopUp.Core.Entities;

namespace TA.TopUp.Infrastructure.DataAccessAbstractions
{
    public class TopUpSystemDbContext : DbContext
    {
        public TopUpSystemDbContext(DbContextOptions<TopUpSystemDbContext> options)
       : base(options)
        {
        }

        public virtual DbSet<Beneficiary> Beneficiaries { get; set; }

        public virtual DbSet<Currency> Currencies { get; set; }

        public virtual DbSet<TopUpOption> TopUpOptions { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<UserTransaction> UserTransactions { get; set; }

        public virtual DbSet<UserWalletBalance> UserWalletBalances { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseSqlServer("server=SHIV;user=LocalDb;password=Test@123;database=TopUp");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Beneficiary>(entity =>
            {
                entity.HasKey(e => e.Uid);

                entity.Property(e => e.Uid).HasColumnName("UId");
                entity.Property(e => e.CountryCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LastUpdatedAt).HasColumnType("datetime");
                entity.Property(e => e.LastUpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.MobileNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.NickName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.User).WithMany(p => p.Beneficiaries)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Beneficiaries_Users");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasKey(e => e.Uid);

                entity.Property(e => e.Uid).HasColumnName("UId");
                entity.Property(e => e.Currency1)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Currency");
            });

            modelBuilder.Entity<TopUpOption>(entity =>
            {
                entity.HasKey(e => e.Uid);

                entity.Property(e => e.Uid).HasColumnName("UId");

                entity.HasOne(d => d.Currency).WithMany(p => p.TopUpOptions)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_TopUpOptions_Currencies");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Uid);

                entity.Property(e => e.Uid).HasColumnName("UId");
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LastUpdatedAt).HasColumnType("datetime");
                entity.Property(e => e.LastUpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserTransaction>(entity =>
            {
                entity.HasKey(e => e.Uid);

                entity.Property(e => e.Uid).HasColumnName("UId");
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LastUpdatedAt).HasColumnType("datetime");
                entity.Property(e => e.LastUpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.TransactionType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Beneficiary).WithMany(p => p.UserTransactions)
                    .HasForeignKey(d => d.BeneficiaryId)
                    .HasConstraintName("FK_UserTransactions_Beneficiaries");

                entity.HasOne(d => d.Currency).WithMany(p => p.UserTransactions)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_UserTransactions_Currencies");

                entity.HasOne(d => d.User).WithMany(p => p.UserTransactions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserTransactions_Users");
            });

            modelBuilder.Entity<UserWalletBalance>(entity =>
            {
                entity.HasKey(e => e.Uid);

                entity.Property(e => e.Uid).HasColumnName("UId");
                entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LastUpdatedAt).HasColumnType("datetime");
                entity.Property(e => e.LastUpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Currency).WithMany(p => p.UserWalletBalances)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_UserWalletBalances_Currencies");

                entity.HasOne(d => d.User).WithMany(p => p.UserWalletBalances)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserWalletBalances_Users");
            });


        }


    }
}
