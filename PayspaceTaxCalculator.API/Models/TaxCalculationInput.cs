using PayspaceTaxCalculator.Domain;

namespace PayspaceTaxCalculator.API.Models
{
    public class TaxCalculationInput
    {
        public string PostalCode { get; set; }
        public decimal AnnualIncome { get; set; }
    }
}
