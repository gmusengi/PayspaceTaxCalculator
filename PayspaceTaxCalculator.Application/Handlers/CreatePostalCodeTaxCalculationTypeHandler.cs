using MediatR;
using PayspaceTaxCalculator.Application.Command;
using PayspaceTaxCalculator.Domain;
using PayspaceTaxCalculator.Domain.DTOs;
using PayspaceTaxCalculator.Domain.Interfaces;
using PayspaceTaxCalculator.Domain.Mappers;

namespace PayspaceTaxCalculator.Application.Handlers
{
    public class CreatePostalCodeTaxCalculationTypeHandler : IRequestHandler<CreatePostalCodeTaxCalculationTypeCommand, PayspaceResponse<PostalCodeTaxCalculationTypeDTO>>
    {
        private readonly IRepository repo;

        public CreatePostalCodeTaxCalculationTypeHandler(IRepository repo)
        {
            this.repo = repo;
        }
        public async Task<PayspaceResponse<PostalCodeTaxCalculationTypeDTO>> Handle(CreatePostalCodeTaxCalculationTypeCommand request, CancellationToken cancellationToken)
        {
            PayspaceResponse<PostalCodeTaxCalculationType> response = PostalCodeTaxCalculationType.Create(request.PostalCode, request.TaxCalculationType);
            if (!response.Success)
                return new PayspaceResponse<PostalCodeTaxCalculationTypeDTO>
                {
                    Id = response.Id,
                    Success = response.Success,
                    Response = response.Response.Map(),
                    ErrorMessage = response.ErrorMessage
                };
            PayspaceResponse<PostalCodeTaxCalculationType> saveResult = await repo.SavePostalCodeTaxCalculationType(response.Response);
            if (saveResult == null)
                return new PayspaceResponse<PostalCodeTaxCalculationTypeDTO> { ErrorMessage = "Failed to save postal code tax calculation type.", Success = false };
            return new PayspaceResponse<PostalCodeTaxCalculationTypeDTO>
            {
                Id = saveResult.Id,
                Success = saveResult.Success,
                Response = saveResult.Response.Map(),
                ErrorMessage = saveResult.ErrorMessage
            };
        }
    }
}
