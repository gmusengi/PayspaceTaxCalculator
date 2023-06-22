using MediatR;
using PayspaceTaxCalculator.Domain;
using PayspaceTaxCalculator.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayspaceTaxCalculator.Application.Command
{
    public class CreateProgressiveTaxRateCommand: IRequest<PayspaceResponse<ProgressiveTaxRateDTO>>
    {
        public CreateProgressiveTaxRateCommand(decimal rate, decimal from, decimal to)
        {
            Rate = rate;
            From = from;
            To = to;
        }

        public decimal Rate { get; }
        public decimal From { get; }
        public decimal To { get; }
    }
}
