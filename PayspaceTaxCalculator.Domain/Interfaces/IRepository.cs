using PayspaceTaxCalculator.Domain.DTOs;

namespace PayspaceTaxCalculator.Domain.Interfaces
{
    public interface IRepository
    {
        Task<PayspaceResponse<List<ProgressiveTaxRate>>> GetProgressiveTaxRates();
        Task<PayspaceResponse<PostalCodeTaxCalculationType>> GetTaxCalculationTypeFromPostalCode(string postalCode);
        Task<PayspaceResponse<PostalCodeTaxCalculationType>> SavePostalCodeTaxCalculationType(PostalCodeTaxCalculationType postalCodeTaxCalculationType);
        Task<PayspaceResponse<ProgressiveTaxRate>> SaveProgressiveTaxRate(ProgressiveTaxRate? progressiveTaxRate);
        Task<PayspaceResponse<TaxCalculator>> SaveTaxCalculation(TaxCalculator? taxCalculator);
    }
}
