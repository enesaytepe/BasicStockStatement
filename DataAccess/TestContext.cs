using Data.Models;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public partial class TestContext : DbContext
    {
        public TestContext(DbContextOptions<TestContext> options)
            : base(options)
        {
        }

        //Tables
        public DbSet<Sti> Stis { get; set; }
        public DbSet<Stk> Stks { get; set; }

        //Stored Procedures
        public DbSet<StockMovement> StockMovements { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // Prosedürü bağlama
        //    modelBuilder.Entity<StockMovement>().HasNoKey();
        //    modelBuilder.Query<StockMovement>().ToView("GetStockMovements");
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer();
            }
        }
    }
}
