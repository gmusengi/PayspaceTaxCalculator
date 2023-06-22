using PayspaceTaxCalculator.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayspaceTaxCalculator.Domain.Mappers
{
    public static class ProgressiveTaxRateMapper
    {
        public static ProgressiveTaxRateDTO Map(this ProgressiveTaxRate progressiveTaxRate)
        {
            return new ProgressiveTaxRateDTO
            {
                Id= progressiveTaxRate.Id,
                Rate = progressiveTaxRate.Rate,
                From = progressiveTaxRate.From,
                To = progressiveTaxRate.To
            };
        }
    }
}
