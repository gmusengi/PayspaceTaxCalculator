using PayspaceTaxCalculator.Domain.DTOs;
using System.ComponentModel.DataAnnotations;

namespace PayspaceTaxCalculator.Domain
{
    public class PostalCodeTaxCalculationType : Entity
    {
        [MaxLength(4)]
        public string PostalCode { get; private set; }
        public TaxCalculationType TaxCalculationType { get; private set; }

        private PostalCodeTaxCalculationType(string postalCode, TaxCalculationType taxCalculationType)
        {
            PostalCode = postalCode;
            TaxCalculationType = taxCalculationType;
        }
        public static PayspaceResponse<PostalCodeTaxCalculationType> Create(string postalCode, TaxCalculationType taxCalculationType)
        {
            if (string.IsNullOrEmpty(postalCode))
                return new PayspaceResponse<PostalCodeTaxCalculationType> { ErrorMessage = "Postal code is required", Success = false };
            return new PayspaceResponse<PostalCodeTaxCalculationType> { Response = new PostalCodeTaxCalculationType(postalCode, taxCalculationType), Success = true };
        }

        public async Task<PayspaceResponse<PostalCodeTaxCalculationType>> Update(string postalCode, TaxCalculationType taxCalculationType)
        {
            if (string.IsNullOrEmpty(postalCode))
                return new PayspaceResponse<PostalCodeTaxCalculationType> { ErrorMessage = "Postal code is required", Success = false };
            PostalCode = postalCode;
            TaxCalculationType = taxCalculationType;
            return new PayspaceResponse<PostalCodeTaxCalculationType> { Response = this, Success = true };
        }
    }
}
