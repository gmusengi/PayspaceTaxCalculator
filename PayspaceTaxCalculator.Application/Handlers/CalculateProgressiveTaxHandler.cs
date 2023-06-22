using MediatR;
using PayspaceTaxCalculator.Application.Command;
using PayspaceTaxCalculator.Domain;
using PayspaceTaxCalculator.Domain.DTOs;
using PayspaceTaxCalculator.Domain.Interfaces;
using PayspaceTaxCalculator.Domain.Mappers;

namespace PayspaceTaxCalculator.Application.Handlers
{
    public class CalculateProgressiveTaxHandler : IRequestHandler<CalculateProgressiveTaxCommand, PayspaceResponse<TaxCalculatorDTO>>
    {
        private readonly IRepository repo;

        public CalculateProgressiveTaxHandler(IRepository repo)
        {
            this.repo = repo;
        }
        public async Task<PayspaceResponse<TaxCalculatorDTO>> Handle(CalculateProgressiveTaxCommand request, CancellationToken cancellationToken)
        {
            PayspaceResponse<TaxCalculator> taxCalculatorResponse = await TaxCalculator.CalculateProgressiveRateTax(request.PostalCode, request.AnnualAmount, request.TaxRates);
            if (!taxCalculatorResponse.Success)
                return new PayspaceResponse<TaxCalculatorDTO>
                {
                    Success = false,
                    ErrorMessage = taxCalculatorResponse.ErrorMessage
                };
            var saveResult = await repo.SaveTaxCalculation((taxCalculatorResponse.Response));
            return new PayspaceResponse<TaxCalculatorDTO>
            {
                Id = saveResult.Id,
                Success = saveResult.Success,
                Response = saveResult.Response.Map(),
                ErrorMessage = saveResult.ErrorMessage
            };
        }
    }
}
