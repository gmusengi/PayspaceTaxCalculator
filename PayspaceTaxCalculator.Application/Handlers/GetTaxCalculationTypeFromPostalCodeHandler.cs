using MediatR;
using PayspaceTaxCalculator.Application.Queries;
using PayspaceTaxCalculator.Domain;
using PayspaceTaxCalculator.Domain.DTOs;
using PayspaceTaxCalculator.Domain.Interfaces;
using PayspaceTaxCalculator.Domain.Mappers;

namespace PayspaceTaxCalculator.Application.Handlers
{
    public class GetTaxCalculationTypeFromPostalCodeHandler : IRequestHandler<GetTaxCalculationTypeFromPostalCodeQuery, PayspaceResponse<PostalCodeTaxCalculationTypeDTO>>
    {
        private readonly IRepository repo;

        public GetTaxCalculationTypeFromPostalCodeHandler(IRepository repo)
        {
            this.repo = repo;
        }
        public async Task<PayspaceResponse<PostalCodeTaxCalculationTypeDTO>> Handle(GetTaxCalculationTypeFromPostalCodeQuery request, CancellationToken cancellationToken)
        {
            PayspaceResponse<PostalCodeTaxCalculationType> result = await repo.GetTaxCalculationTypeFromPostalCode(request.PostalCode);
            if (!result.Success)
                return new PayspaceResponse<PostalCodeTaxCalculationTypeDTO>
                {
                    ErrorMessage = result.ErrorMessage,
                    Success = false
                };
            return new PayspaceResponse<PostalCodeTaxCalculationTypeDTO>
            {
                ErrorMessage = result.ErrorMessage,
                Id = result.Id,
                Response = result.Response.Map(),
                Success = result.Success
            };

        }
    }
}
