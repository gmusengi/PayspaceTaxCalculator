using Microsoft.Extensions.Configuration;
using PayspaceTaxCalculator.Domain.DTOs;

namespace PayspaceTaxCalculator.Domain
{
    public class TaxCalculator : Entity
    {
        const decimal FlatValueAnnualIncomeThreshold = 200000;
        public string PostalCode { get; private set; }
        public decimal AnnualIncome { get; private set; }
        public decimal CalculatedTaxAmount { get; private set; }
        public DateTime Date { get; set; } = DateTime.Now;
        private TaxCalculator(string postalCode, decimal annualIncome, decimal calculatedTaxAmount)
        {
            PostalCode = postalCode;
            AnnualIncome = annualIncome;
            CalculatedTaxAmount = calculatedTaxAmount;
        }
        public static async Task<PayspaceResponse<TaxCalculator>> CalculateFlatValueTax(string postalCode, decimal annualIncome, IConfiguration configuration)
        {
            if (string.IsNullOrEmpty(postalCode))
                return new PayspaceResponse<TaxCalculator> { ErrorMessage = "Postal code is required", Success = false };
            if (annualIncome <= 0)
                return new PayspaceResponse<TaxCalculator> { ErrorMessage = "Annual income must be greater than zero", Success = false };
            if (string.IsNullOrEmpty(configuration.GetSection("FlatValuePerYear").Value))
                return new PayspaceResponse<TaxCalculator> { ErrorMessage = "Flat Value Per Year has not been configured", Success = false };
            if (annualIncome < FlatValueAnnualIncomeThreshold && string.IsNullOrEmpty(configuration.GetSection("FlatValuePerYearBelowTwoHundredThousand").Value))
                return new PayspaceResponse<TaxCalculator> { ErrorMessage = $"Flat Value Tax Percentage for below {FlatValueAnnualIncomeThreshold} has not been configured." };

            decimal calculatedAmount = 0;
            if (annualIncome >= FlatValueAnnualIncomeThreshold)
                calculatedAmount = 10000;
            else
            {
                decimal flatValuePercent = 0;
                if (!decimal.TryParse(configuration.GetSection("FlatValuePerYearBelowTwoHundredThousand").Value, out flatValuePercent))
                    return new PayspaceResponse<TaxCalculator> { ErrorMessage = $"Flat Value Tax Percentage for below {FlatValueAnnualIncomeThreshold} is not valid." };
                calculatedAmount = annualIncome * (flatValuePercent / 100M);
            }
            return await Task.FromResult(new PayspaceResponse<TaxCalculator> { Response = new TaxCalculator(postalCode, annualIncome, calculatedAmount), Success = true });
        }
        public async Task<PayspaceResponse<TaxCalculator>> Update(decimal annualIncome, string postalCode, decimal calculatedTaxAmount, IConfiguration configuration)
        {
            if (string.IsNullOrEmpty(postalCode))
                return new PayspaceResponse<TaxCalculator> { ErrorMessage = "Postal code is required", Success = false };
            if (annualIncome <= 0)
                return new PayspaceResponse<TaxCalculator> { ErrorMessage = "Annual income must be greater than zero", Success = false };
            if (string.IsNullOrEmpty(configuration.GetSection("FlatValuePerYear").Value))
                return new PayspaceResponse<TaxCalculator> { ErrorMessage = "Flat Value Per Year has not been configured", Success = false };
            if (annualIncome < FlatValueAnnualIncomeThreshold && string.IsNullOrEmpty(configuration.GetSection("FlatValuePerYearBelowTwoHundredThousand").Value))
                return new PayspaceResponse<TaxCalculator> { ErrorMessage = $"Flat Value Tax Percentage for below {FlatValueAnnualIncomeThreshold} has not been configured." };

            PostalCode = postalCode;
            AnnualIncome = annualIncome;
            CalculatedTaxAmount = calculatedTaxAmount;
            return new PayspaceResponse<TaxCalculator> { Response = this, Success = true };
        }

        public static async Task<PayspaceResponse<TaxCalculator>> CalculateFlatRateTax(string postalCode, decimal annualIncome, IConfiguration configuration)
        {
            if (string.IsNullOrEmpty(postalCode))
                return new PayspaceResponse<TaxCalculator> { ErrorMessage = "Postal code is required", Success = false };
            if (annualIncome <= 0)
                return new PayspaceResponse<TaxCalculator> { ErrorMessage = "Annual income must be greater than zero", Success = false };
            if (string.IsNullOrEmpty(configuration.GetSection("FlatRate").Value))
                return new PayspaceResponse<TaxCalculator> { ErrorMessage = "Flat Rate has not been configured", Success = false };

            decimal calculatedAmount = 0;
            decimal flatRatePercent = 0;
            if (!decimal.TryParse(configuration.GetSection("FlatRate").Value, out flatRatePercent))
                return new PayspaceResponse<TaxCalculator> { ErrorMessage = $"Flat Rate Percentage is not valid." };
            calculatedAmount = annualIncome * (flatRatePercent / 100M);
            return await Task.FromResult(new PayspaceResponse<TaxCalculator> { Response = new TaxCalculator(postalCode, annualIncome, calculatedAmount), Success = true });
        }

        public static async Task<PayspaceResponse<TaxCalculator>> CalculateProgressiveRateTax(string postalCode, decimal annualIncome, List<ProgressiveTaxRateDTO> taxRates)
        {
            if (string.IsNullOrEmpty(postalCode))
                return new PayspaceResponse<TaxCalculator> { ErrorMessage = "Postal code is required", Success = false };
            if (annualIncome <= 0)
                return new PayspaceResponse<TaxCalculator> { ErrorMessage = "Annual income must be greater than zero", Success = false };
            decimal taxAmount = 0;
            decimal taxableIncome = annualIncome;
            foreach (var taxRate in taxRates)
            {
                if (taxRate.To == -1)
                    break; //means we've reached the end of the tax rates and the remaining income must be taxed at the last rate
                if (taxableIncome < taxRate.To)
                {
                    taxAmount += taxableIncome * (taxRate.Rate / 100M);
                    taxableIncome = 0; //for the last scenario where the tax rates are exhausted
                    break;
                }
                else
                {
                    taxAmount += taxRate.To * (taxRate.Rate / 100M);
                    taxableIncome -= taxRate.To;
                }
            }
            if (taxableIncome > 0)
                taxAmount += taxableIncome * (taxRates.LastOrDefault().Rate / 100M);
            return await Task.FromResult(new PayspaceResponse<TaxCalculator> { Response = new TaxCalculator(postalCode, annualIncome, taxAmount), Success = true });
        }
    }
}
