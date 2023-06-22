using MediatR;
using PayspaceTaxCalculator.Application.Queries;
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
            var result = await repo.GetProgressiveTaxRates();
            return new PayspaceResponse<List<ProgressiveTaxRateDTO>>
            {
                ErrorMessage = result.ErrorMessage,
                Id = result.Id,
                Response = result.Response.Select(r => r.Map()).ToList(),
                Success = result.Success
            };
        }
    }
}
