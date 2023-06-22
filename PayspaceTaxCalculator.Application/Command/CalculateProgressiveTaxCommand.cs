using MediatR;
using PayspaceTaxCalculator.Domain.DTOs;

namespace PayspaceTaxCalculator.Application.Command
{
    public class CalculateProgressiveTaxCommand: IRequest<PayspaceResponse<TaxCalculatorDTO>>
    {
        public CalculateProgressiveTaxCommand(string postalCode, decimal annualAmount, List<ProgressiveTaxRateDTO> taxRates)
        {
            PostalCode = postalCode;
            AnnualAmount = annualAmount;
            TaxRates = taxRates;
        }

        public string PostalCode { get; }
        public decimal AnnualAmount { get; }
        public List<ProgressiveTaxRateDTO> TaxRates { get; }
    }
}
