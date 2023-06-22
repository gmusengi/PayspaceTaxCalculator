using MediatR;
using Microsoft.AspNetCore.Mvc;
using PayspaceTaxCalculator.API.Models;
using PayspaceTaxCalculator.Application.Command;
using PayspaceTaxCalculator.Application.Queries;
using PayspaceTaxCalculator.Domain;
using PayspaceTaxCalculator.Domain.DTOs;

namespace PayspaceTaxCalculator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxCalculationController : ControllerBase
    {
        private readonly IMediator mediator;

        public TaxCalculationController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        public async Task<PayspaceResponse<TaxCalculatorDTO>> Calculate(TaxCalculationInput taxCalculationInput)
        {
            var taxCalculationTypeResponse = await mediator.Send(new GetTaxCalculationTypeFromPostalCodeQuery(taxCalculationInput.PostalCode), new CancellationToken());
            if (!taxCalculationTypeResponse.Success)
                return new PayspaceResponse<TaxCalculatorDTO>
                {
                    Success = false,
                    ErrorMessage = $"Failed to get tax calculation type for postal code {taxCalculationInput.PostalCode}"
                };
            switch (taxCalculationTypeResponse.Response.TaxCalculationType)
            {
                case TaxCalculationType.FlatValue:
                    var flatValueResult = await mediator.Send(new CalculateFlatValueTaxCommand(taxCalculationInput.PostalCode, taxCalculationInput.AnnualIncome), new CancellationToken());
                    return flatValueResult;
                case TaxCalculationType.FlatRate:
                    var flatRateResult = await mediator.Send(new CalculateFlatRateTaxCommand(taxCalculationInput.PostalCode, taxCalculationInput.AnnualIncome), new CancellationToken());
                    return flatRateResult;
                case TaxCalculationType.Progressive:
                    var taxRatesResponse = await mediator.Send(new GetProgressiveTaxRatesQuery(), new CancellationToken());
                    if (!taxRatesResponse.Success)
                        return new PayspaceResponse<TaxCalculatorDTO>
                        {
                            Id = taxRatesResponse.Id,
                            Success = false,
                            ErrorMessage = taxRatesResponse.ErrorMessage
                        };
                    var progressiveResult = await mediator.Send(new CalculateProgressiveTaxCommand(taxCalculationInput.PostalCode, taxCalculationInput.AnnualIncome, taxRatesResponse.Response), new CancellationToken());
                    return progressiveResult;
                default:
                    return new PayspaceResponse<TaxCalculatorDTO> { ErrorMessage = $"Invalid tax calculation type {taxCalculationTypeResponse.Response.TaxCalculationType}", Success = false };
            }
        }
    }
}
