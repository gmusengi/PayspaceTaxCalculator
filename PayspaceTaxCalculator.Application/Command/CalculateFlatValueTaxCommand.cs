using MediatR;
using PayspaceTaxCalculator.Domain.DTOs;

namespace PayspaceTaxCalculator.Application.Command
{
    public class CalculateFlatValueTaxCommand: IRequest<PayspaceResponse<TaxCalculatorDTO>>
    {
        public CalculateFlatValueTaxCommand(string postalCode, decimal annualAmount)
        {
            PostalCode = postalCode;
            AnnualAmount = annualAmount;
        }

        public string PostalCode { get; }
        public decimal AnnualAmount { get; }
    }
}
