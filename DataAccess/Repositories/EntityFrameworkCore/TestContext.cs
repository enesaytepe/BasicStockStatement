﻿using Common;
using Data.Models;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.EntityFrameworkCore
{
    public partial class TestContext : DbContext
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public TestContext(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        //Tables
        public virtual DbSet<Sti> Stis { get; set; } = null!;
        public virtual DbSet<Stk> Stks { get; set; } = null!;

        //Stored Procedures
        public DbSet<StockMovement> StockMovements { get; set; }
        public DbSet<StockMovement> PagedStockMovements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionStringProvider.DefaultConnectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Turkish_CI_AS");

            modelBuilder.Entity<Sti>(entity =>
            {
                entity.HasKey(e => new { e.EvrakNo, e.Tarih, e.IslemTur })
                    .HasName("pkSTI");

                entity.ToTable("STI");

                entity.Property(e => e.EvrakNo)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Birim)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Fiyat).HasColumnType("numeric(25, 6)");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.MalKodu)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Miktar).HasColumnType("numeric(25, 6)");

                entity.Property(e => e.Tutar).HasColumnType("numeric(25, 6)");
            });

            modelBuilder.Entity<Stk>(entity =>
            {
                entity.HasKey(e => e.MalKodu)
                    .HasName("pkSTK");

                entity.ToTable("STK");

                entity.Property(e => e.MalKodu)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.MalAdi)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            // Prosedürü bağlama
            modelBuilder.Entity<StockMovement>().HasKey(s => s.SiraNo);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
