using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PayspaceTaxCalculator.Domain;
using PayspaceTaxCalculator.Domain.DTOs;
using PayspaceTaxCalculator.Domain.Interfaces;
using PayspaceTaxCalculator.Domain.Mappers;

namespace PayspaceTaxCalculator.Infrastructure
{
    public class Repository : IRepository
    {
        private readonly PayspaceDbContext payspaceDbContext;
        private readonly IConfiguration configuration;

        public Repository(PayspaceDbContext payspaceDbContext, IConfiguration configuration)
        {
            this.payspaceDbContext = payspaceDbContext;
            this.configuration = configuration;
        }
        public async Task<PayspaceResponse<List<ProgressiveTaxRate>>> GetProgressiveTaxRates()
        {
            var result = await payspaceDbContext.ProgressiveTaxRates.OrderBy(r => r.From).ToListAsync();
            if (result != null)
                return new PayspaceResponse<List<ProgressiveTaxRate>> { Response = result, Success = true };
            return new PayspaceResponse<List<ProgressiveTaxRate>> { Success = false };
        }
        public async Task<PayspaceResponse<PostalCodeTaxCalculationType>> GetTaxCalculationTypeFromPostalCode(string postalCode)
        {
            var result = await payspaceDbContext.PostalCodeTaxCalculationTypes.FirstOrDefaultAsync(p => p.PostalCode == postalCode);
            if (result == null)
                return new PayspaceResponse<PostalCodeTaxCalculationType> { ErrorMessage = $"Could not find tax calculation type for postal code {postalCode}", Success = false };
            return new PayspaceResponse<PostalCodeTaxCalculationType> { Response = result, Success = true };
        }
        public async Task<PayspaceResponse<PostalCodeTaxCalculationType>> SavePostalCodeTaxCalculationType(PostalCodeTaxCalculationType postalCodeTaxCalculationType)
        {
            var existingRecord = await payspaceDbContext.PostalCodeTaxCalculationTypes.FirstOrDefaultAsync(p => p.Id == postalCodeTaxCalculationType.Id);
            if (existingRecord != null)
            {
                var updatedRecordResponse = await existingRecord.Update(postalCodeTaxCalculationType.PostalCode, postalCodeTaxCalculationType.TaxCalculationType);
                if (!updatedRecordResponse.Success)
                    return updatedRecordResponse;
            }
            else
            {
                payspaceDbContext.PostalCodeTaxCalculationTypes.Add(postalCodeTaxCalculationType);
            }
            await payspaceDbContext.SaveChangesAsync();
            return new PayspaceResponse<PostalCodeTaxCalculationType> { Response = postalCodeTaxCalculationType, Success = true };
        }
        public async Task<PayspaceResponse<ProgressiveTaxRate>> SaveProgressiveTaxRate(ProgressiveTaxRate? progressiveTaxRate)
        {
            var existingRecord = await payspaceDbContext.ProgressiveTaxRates.FirstOrDefaultAsync(p => p.Id == progressiveTaxRate.Id);
            if (existingRecord != null)
            {
                var updatedRecordResponse = await existingRecord.Update(progressiveTaxRate.Rate, progressiveTaxRate.From, progressiveTaxRate.To);
                if (!updatedRecordResponse.Success)
                    return updatedRecordResponse;
            }
            else
            {
                payspaceDbContext.ProgressiveTaxRates.Add(progressiveTaxRate);
            }
            await payspaceDbContext.SaveChangesAsync();
            return new PayspaceResponse<ProgressiveTaxRate> { Response = progressiveTaxRate, Success = true };
        }
        public async Task<PayspaceResponse<TaxCalculator>> SaveTaxCalculation(TaxCalculator? taxCalculator)
        {
            if (taxCalculator == null)
                return new PayspaceResponse<TaxCalculator> { ErrorMessage = "Nothing to save", Success = false };
            var existingRecord = await payspaceDbContext.TaxCalculators.FirstOrDefaultAsync(p => p.Id == taxCalculator.Id);
            if (existingRecord != null)
            {
                var updatedRecordResponse = await existingRecord.Update(taxCalculator.AnnualIncome, taxCalculator.PostalCode, taxCalculator.CalculatedTaxAmount, configuration);
                if (!updatedRecordResponse.Success)
                    return updatedRecordResponse;
            }
            else
            {
                payspaceDbContext.TaxCalculators.Add(taxCalculator);
            }
            await payspaceDbContext.SaveChangesAsync();
            return new PayspaceResponse<TaxCalculator> { Response = taxCalculator, Success = true };
        }
    }
}
