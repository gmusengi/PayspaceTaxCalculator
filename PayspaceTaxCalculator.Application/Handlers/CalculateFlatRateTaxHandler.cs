using MediatR;
using Microsoft.Extensions.Configuration;
using PayspaceTaxCalculator.Application.Command;
using PayspaceTaxCalculator.Domain;
using PayspaceTaxCalculator.Domain.DTOs;
using PayspaceTaxCalculator.Domain.Interfaces;
using PayspaceTaxCalculator.Domain.Mappers;

namespace PayspaceTaxCalculator.Application.Handlers
{
    public class CalculateFlatRateTaxHandler : IRequestHandler<CalculateFlatRateTaxCommand, PayspaceResponse<TaxCalculatorDTO>>
    {
        private readonly IRepository repo;
        private readonly IConfiguration configuration;

        public CalculateFlatRateTaxHandler(IRepository repo, IConfiguration configuration)
        {
            this.repo = repo;
            this.configuration = configuration;
        }
        public async Task<PayspaceResponse<TaxCalculatorDTO>> Handle(CalculateFlatRateTaxCommand request, CancellationToken cancellationToken)
        {
            PayspaceResponse<TaxCalculator> taxCalculatorResponse = await TaxCalculator.CalculateFlatRateTax(request.PostalCode, request.AnnualAmount, configuration);
            if (!taxCalculatorResponse.Success)
                return new PayspaceResponse<TaxCalculatorDTO>
                {
                    Id = taxCalculatorResponse.Id,
                    Success = taxCalculatorResponse.Success,
                    Response = taxCalculatorResponse.Response.Map(),
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
