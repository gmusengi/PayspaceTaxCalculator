using PayspaceTaxCalculator.Domain.DTOs;

namespace PayspaceTaxCalculator.Services
{
    public interface IAPIService
    {
        Task<PayspaceResponse<TaxCalculatorDTO>> CalculateTax(string postalCode, decimal annualIncome);
    }
}
