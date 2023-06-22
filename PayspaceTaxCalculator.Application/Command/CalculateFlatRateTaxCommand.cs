using MediatR;
using PayspaceTaxCalculator.Domain.DTOs;

namespace PayspaceTaxCalculator.Application.Command
{
    public class CalculateFlatRateTaxCommand: IRequest<PayspaceResponse<TaxCalculatorDTO>>
    {
        public CalculateFlatRateTaxCommand(string postalCode, decimal annualAmount)
        {
            PostalCode = postalCode;
            AnnualAmount = annualAmount;
        }

        public string PostalCode { get; }
        public decimal AnnualAmount { get; }
    }
}
