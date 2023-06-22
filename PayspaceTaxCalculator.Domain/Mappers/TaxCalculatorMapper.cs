using PayspaceTaxCalculator.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayspaceTaxCalculator.Domain.Mappers
{
    public static class TaxCalculatorMapper
    {
        public static TaxCalculatorDTO Map(this TaxCalculator taxCalculator)
        {
            return new TaxCalculatorDTO
            {
                Id = taxCalculator.Id,
                AnnualIncome = taxCalculator.AnnualIncome,
                CalculatedTaxAmount = taxCalculator.CalculatedTaxAmount,
                PostalCode = taxCalculator.PostalCode
            };
        }
    }
}
