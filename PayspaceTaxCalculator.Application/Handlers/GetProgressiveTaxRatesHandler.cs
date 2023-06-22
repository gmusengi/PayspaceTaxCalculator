using MediatR;
using PayspaceTaxCalculator.Application.Queries;
using PayspaceTaxCalculator.Domain;
using PayspaceTaxCalculator.Domain.DTOs;
using PayspaceTaxCalculator.Domain.Interfaces;
using PayspaceTaxCalculator.Domain.Mappers;

namespace PayspaceTaxCalculator.Application.Handlers
{
    public class GetProgressiveTaxRatesHandler : IRequestHandler<GetProgressiveTaxRatesQuery, PayspaceResponse<List<ProgressiveTaxRateDTO>>>
    {
        private readonly IRepository repo;

        public GetProgressiveTaxRatesHandler(IRepository repo)
        {
            this.repo = repo;
        }
        public async Task<PayspaceResponse<List<ProgressiveTaxRateDTO>>> Handle(GetProgressiveTaxRatesQuery request, CancellationToken cancellationToken)
        {
            PayspaceResponse<List<ProgressiveTaxRate>> response = await repo.GetProgressiveTaxRates();
            if (!response.Success)
                return new PayspaceResponse<List<ProgressiveTaxRateDTO>>
                {
                    ErrorMessage = response.ErrorMessage,
                    Success = false
                };
            return new PayspaceResponse<List<ProgressiveTaxRateDTO>>
            {
                ErrorMessage = response.ErrorMessage,
                Id = response.Id,
                Response = response.Response.Select(r => r.Map()).ToList(),
                Success = response.Success
            };
        }
    }
}
