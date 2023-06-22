using MediatR;
using Microsoft.Extensions.Configuration;
using PayspaceTaxCalculator.Application.Command;
using PayspaceTaxCalculator.Domain;
using PayspaceTaxCalculator.Domain.DTOs;
using PayspaceTaxCalculator.Domain.Interfaces;
using PayspaceTaxCalculator.Domain.Mappers;

namespace PayspaceTaxCalculator.Application.Handlers
{
    public class CalculateFlatValueTaxHandler : IRequestHandler<CalculateFlatValueTaxCommand, PayspaceResponse<TaxCalculatorDTO>>
    {
        private readonly IRepository repo;
        private readonly IConfiguration configuration;

        public CalculateFlatValueTaxHandler(IRepository repo, IConfiguration configuration)
        {
            this.repo = repo;
            this.configuration = configuration;
        }
        public async Task<PayspaceResponse<TaxCalculatorDTO>> Handle(CalculateFlatValueTaxCommand request, CancellationToken cancellationToken)
        {
            PayspaceResponse<TaxCalculator> taxCalculatorResponse = await TaxCalculator.CalculateFlatValueTax(request.PostalCode, request.AnnualAmount, configuration);
            if (!taxCalculatorResponse.Success)
                return new PayspaceResponse<TaxCalculatorDTO>
                {
                    Id = taxCalculatorResponse.Response.Id,
                    Response = taxCalculatorResponse.Response.Map(),
                    ErrorMessage = taxCalculatorResponse.ErrorMessage,
                    Success = taxCalculatorResponse.Success
                };
            var saveResult = await repo.SaveTaxCalculation(taxCalculatorResponse.Response);
            return new PayspaceResponse<TaxCalculatorDTO>
            {
                Id = saveResult.Response.Id,
                Response = saveResult.Response.Map(),
                ErrorMessage = saveResult.ErrorMessage,
                Success = saveResult.Success
            };
        }
    }
}
