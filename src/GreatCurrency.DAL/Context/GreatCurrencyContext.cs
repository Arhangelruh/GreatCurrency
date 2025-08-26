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
        /// City.
        /// </summary>
        public DbSet<City> Cities { get; set; }

        /// <summary>
        /// Banks.
        /// </summary>
        public DbSet<Bank> Banks { get; set; }

        /// <summary>
        /// Bank departments.
        /// </summary>
        public DbSet<BankDepartment> BankDepartments { get; set; }

        /// <summary>
        /// Request.
        /// </summary>
        public DbSet<Request> Requests { get; set; }

        /// <summary>
        /// Currencies.
        /// </summary>
        public DbSet<Currency> Currency { get; set; }

        /// <summary>
        /// Best currency.
        /// </summary>
        public DbSet<BestCurrency> BestCurrencies { get; set; }

		/// <summary>
		/// Best requests to sevices.
		/// </summary>
		public DbSet<SCRequest> SCRequests { get; set; }

		/// <summary>
		/// Bank currecy sevices.
		/// </summary>
		public DbSet<CurrencyService> CurrencyServices { get; set; }

		/// <summary>
		/// Bank currecy sevices currency.
		/// </summary>
		public DbSet<CSCurrency> CSCurrencies { get; set; }

		/// <summary>
		/// Requests for legal entities.
		/// </summary>
		public DbSet<LERequest> LERequests { get; set; }

		/// <summary>
		/// Currecies for legal entities.
		/// </summary>
		public DbSet<LECurrency> LECurrencies { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.ApplyConfiguration(new CityConfiguration());
            modelBuilder.ApplyConfiguration(new BankConfiguration());
            modelBuilder.ApplyConfiguration(new RequestConfiguration());
            modelBuilder.ApplyConfiguration(new BankDepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new BestCurrencyConfiguration());
            modelBuilder.ApplyConfiguration(new CurrencyConfiguration());
			modelBuilder.ApplyConfiguration(new SCRequestConfiguration());
            modelBuilder.ApplyConfiguration(new CurrencyServiceConfiguration());
			modelBuilder.ApplyConfiguration(new CSCurrenciesConfiguration());
			modelBuilder.ApplyConfiguration(new LECurrenciesConfiguration());
			modelBuilder.ApplyConfiguration(new LERequestConfiguration());

			base.OnModelCreating(modelBuilder);
        }
    }
}
