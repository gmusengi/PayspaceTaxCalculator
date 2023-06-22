using MediatR;
using PayspaceTaxCalculator.Application.Command;
using PayspaceTaxCalculator.Domain;
using PayspaceTaxCalculator.Domain.DTOs;
using PayspaceTaxCalculator.Domain.Interfaces;
using PayspaceTaxCalculator.Domain.Mappers;

namespace PayspaceTaxCalculator.Application.Handlers
{
    public class CreateProgressiveTaxRateHandler : IRequestHandler<CreateProgressiveTaxRateCommand, PayspaceResponse<ProgressiveTaxRateDTO>>
    {
        private readonly IRepository repo;

        public CreateProgressiveTaxRateHandler(IRepository repo)
        {
            this.repo = repo;
        }
        public async Task<PayspaceResponse<ProgressiveTaxRateDTO>> Handle(CreateProgressiveTaxRateCommand request, CancellationToken cancellationToken)
        {
            PayspaceResponse<ProgressiveTaxRate> response = await ProgressiveTaxRate.Create(request.Rate, request.From, request.To);
            if (!response.Success)
                return new PayspaceResponse<ProgressiveTaxRateDTO>
                {
                    Id = response.Id,
                    ErrorMessage = response.ErrorMessage,
                    Response = response.Response.Map(),
                    Success = response.Success
                };
            PayspaceResponse<ProgressiveTaxRate> saveResult = await repo.SaveProgressiveTaxRate(response.Response);
            if (saveResult == null)
                return new PayspaceResponse<ProgressiveTaxRateDTO> { ErrorMessage = "Failed to save progressive tax rate.", Success = false };
            return new PayspaceResponse<ProgressiveTaxRateDTO>
            {
                Id = saveResult.Id,
                ErrorMessage = saveResult.ErrorMessage,
                Response = saveResult.Response.Map(),
                Success = saveResult.Success
            };

        }
    }
}
