using PayspaceTaxCalculator.Domain;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PayspaceTaxCalculator.API.Models
{
    public class TaxCalculationInput
    {
        [MaxLength(4)]
        [Required]
        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }
        [Required]
        [DisplayName("Annual Income")]
        public decimal AnnualIncome { get; set; }
    }
}
