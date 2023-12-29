using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BorrowerPanel1.Models
{
    public partial class FinalDbContext : DbContext
    {
        public FinalDbContext()
        {
        }

        public FinalDbContext(DbContextOptions<FinalDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BorrowerTb> BorrowerTb { get; set; }
        public virtual DbSet<Instruments> Instruments { get; set; }
        public virtual DbSet<LenderTb> LenderTb { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<RequestRental> RequestRental { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                ConfigurationBuilder confBuilder = new ConfigurationBuilder();
                optionsBuilder.UseSqlServer(confBuilder.Build().GetSection("ConnectionStrings:DefaultConnection").Value);

            }
       
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BorrowerTb>(entity =>
            {
                entity.HasKey(e => e.BorrowerId);

                entity.ToTable("BorrowerTB");

                entity.Property(e => e.BorrowerId).HasColumnName("BorrowerID");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmailId)
                    .HasColumnName("EmailID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Instruments>(entity =>
            {
                entity.HasKey(e => e.InstrumentId);

                entity.Property(e => e.InstrumentId).HasColumnName("InstrumentID");

                entity.Property(e => e.AvailabilityStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Image).HasColumnType("text");

                entity.Property(e => e.InstrumentName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LenderId).HasColumnName("LenderID");

                entity.HasOne(d => d.Lender)
                    .WithMany(p => p.Instruments)
                    .HasForeignKey(d => d.LenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Instruments_LenderTB");
            });

            modelBuilder.Entity<LenderTb>(entity =>
            {
                entity.HasKey(e => e.LenderId);

                entity.ToTable("LenderTB");

                entity.Property(e => e.LenderId).HasColumnName("LenderID");

                entity.Property(e => e.Address).HasColumnType("text");

                entity.Property(e => e.EmailId)
                    .HasColumnName("EmailID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.Message1)
                    .HasColumnName("Message")
                    .HasColumnType("text");

                entity.HasOne(d => d.Burrower)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.BurrowerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Message_BorrowerTB");
            });

            modelBuilder.Entity<RequestRental>(entity =>
            {
                entity.HasKey(e => e.ReqId);

                entity.Property(e => e.ReqId).HasColumnName("ReqID");

                entity.Property(e => e.BorrowerId).HasColumnName("BorrowerID");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.Experience)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InstrumentId).HasColumnName("InstrumentID");

                entity.Property(e => e.RequestDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Instrument)
                    .WithMany(p => p.RequestRental)
                    .HasForeignKey(d => d.InstrumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestRental_Instruments");
            });

            modelBuilder.Entity<Transactions>(entity =>
            {
                entity.HasKey(e => e.TransactionId);

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

                entity.Property(e => e.BorrowerId).HasColumnName("BorrowerID");

                entity.Property(e => e.LenderId).HasColumnName("LenderID");

                entity.Property(e => e.ReqId).HasColumnName("ReqID");

                entity.Property(e => e.TransactionDate)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Borrower)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.BorrowerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transactions_BorrowerTB");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
