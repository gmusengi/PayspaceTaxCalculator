using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayspaceTaxCalculator.Domain.DTOs
{
    public class PostalCodeTaxCalculationTypeDTO
    {
        public Guid Id { get; set; }
        public string PostalCode { get; set; }
        public TaxCalculationType TaxCalculationType { get; set; }
    }
}
