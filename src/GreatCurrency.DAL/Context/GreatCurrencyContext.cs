using GreatCurrency.DAL.Configurations;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GreatCurrency.DAL.Context
{
    public class GreatCurrencyContext : DbContext
    {
        public GreatCurrencyContext(DbContextOptions<GreatCurrencyContext> options)
        : base(options)
        {        
        }

        /// <summary>
        /// Banks.
        /// </summary>
        public DbSet<Bank> Banks { get; set; }

        /// <summary>
        /// Bank departments.
        /// </summary>
        public DbSet<BankDepartment> BankDepartments { get; set; }

        /// <summary>
        /// Currencies.
        /// </summary>
        public DbSet<Currency> Currency { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.ApplyConfiguration(new BankConfiguration());
            modelBuilder.ApplyConfiguration(new BankDepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new CurrencyConfiguration());
            modelBuilder.ApplyConfiguration(new CityConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
