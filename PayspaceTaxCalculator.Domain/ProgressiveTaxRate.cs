using PayspaceTaxCalculator.Domain.DTOs;

namespace PayspaceTaxCalculator.Domain
{
    public class ProgressiveTaxRate: Entity
    {
        private ProgressiveTaxRate(decimal rate, decimal from, decimal to)
        {
            Rate = rate;
            From = from;
            To = to;
        }

        public decimal Rate { get; private set; }
        public decimal From { get; private set; }
        public decimal To { get; private set; }

        public static async Task<PayspaceResponse<ProgressiveTaxRate>> Create(decimal rate, decimal from, decimal to)
        {
            if (rate <= 0)
                return new PayspaceResponse<ProgressiveTaxRate> { ErrorMessage = "Rate must be greater than zero", Success = false };
            if (from < 0)
                return new PayspaceResponse<ProgressiveTaxRate> { ErrorMessage = "From amount must be greater than or equal to zero", Success = false };
            if (to <= 0)
                return new PayspaceResponse<ProgressiveTaxRate> { ErrorMessage = "To amount must be greater than zero", Success = false };

            return new PayspaceResponse<ProgressiveTaxRate> { Response = new ProgressiveTaxRate(rate, from, to), Success = true };
        }

        public async Task<PayspaceResponse<ProgressiveTaxRate>> Update(decimal rate, decimal from, decimal to)
        {
            if (rate <= 0)
                return new PayspaceResponse<ProgressiveTaxRate> { ErrorMessage = "Rate must be greater than zero", Success = false };
            if (from < 0)
                return new PayspaceResponse<ProgressiveTaxRate> { ErrorMessage = "From amount must be greater than or equal to zero", Success = false };
            if (to <= 0)
                return new PayspaceResponse<ProgressiveTaxRate> { ErrorMessage = "To amount must be greater than zero", Success = false };

            Rate = rate;
            From = from;
            To = to;
            return new PayspaceResponse<ProgressiveTaxRate> { Response = this, Success = true };
        }
    }
}
