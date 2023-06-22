using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayspaceTaxCalculator.Domain.DTOs
{
    public class TaxCalculatorDTO: BaseDTO
    {
        public Guid Id { get; set; }
        public string PostalCode { get; set; }
        public decimal AnnualIncome { get; set; }
        public decimal CalculatedTaxAmount { get; set; }
    }
}
