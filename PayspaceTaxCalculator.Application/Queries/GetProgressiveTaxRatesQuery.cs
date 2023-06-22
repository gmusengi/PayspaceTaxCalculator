using MediatR;
using PayspaceTaxCalculator.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayspaceTaxCalculator.Application.Queries
{
    public class GetProgressiveTaxRatesQuery: IRequest<PayspaceResponse<List<ProgressiveTaxRateDTO>>>
    {
    }
}
