using MediatR;
using PayspaceTaxCalculator.Domain;
using PayspaceTaxCalculator.Domain.DTOs;

namespace PayspaceTaxCalculator.Application.Command
{
    public class CreatePostalCodeTaxCalculationTypeCommand: IRequest<PayspaceResponse<PostalCodeTaxCalculationTypeDTO>>
    {

        public CreatePostalCodeTaxCalculationTypeCommand(string postalCode, TaxCalculationType taxCalculationType)
        {
            PostalCode = postalCode;
            TaxCalculationType = taxCalculationType;
        }

        public string PostalCode { get; }
        public TaxCalculationType TaxCalculationType { get; }
    }
}
