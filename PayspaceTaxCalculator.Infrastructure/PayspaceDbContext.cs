using Microsoft.EntityFrameworkCore;
using PayspaceTaxCalculator.Domain;

namespace PayspaceTaxCalculator.Infrastructure
{
    public class PayspaceDbContext : DbContext
    {
        public PayspaceDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<PostalCodeTaxCalculationType> PostalCodeTaxCalculationTypes { get; set; }
        public DbSet<ProgressiveTaxRate> ProgressiveTaxRates { get; set; }
        public DbSet<TaxCalculator> TaxCalculators { get; set; }
    }
}