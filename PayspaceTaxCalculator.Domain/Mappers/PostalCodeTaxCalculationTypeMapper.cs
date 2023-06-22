using PayspaceTaxCalculator.Domain.DTOs;

namespace PayspaceTaxCalculator.Domain.Mappers
{
    public static class PostalCodeTaxCalculationTypeMapper
    {
        public static PostalCodeTaxCalculationTypeDTO Map(this PostalCodeTaxCalculationType postalCodeTaxCalculationType)
        {
            return new PostalCodeTaxCalculationTypeDTO
            {
                Id = postalCodeTaxCalculationType.Id,
                PostalCode = postalCodeTaxCalculationType.PostalCode,
                TaxCalculationType = postalCodeTaxCalculationType.TaxCalculationType
            };
        }
    }
}
