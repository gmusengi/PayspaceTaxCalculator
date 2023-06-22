using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayspaceTaxCalculator.Domain.DTOs
{
    public class ProgressiveTaxRateDTO
    {
        public Guid Id { get; set; }
        public decimal From { get; set; }
        public decimal To { get; set; }
        public decimal Rate { get; set; }
    }
}
