using Microsoft.Extensions.Configuration;
using Moq;
using PayspaceTaxCalculator.Application.Handlers;
using PayspaceTaxCalculator.Domain;
using PayspaceTaxCalculator.Domain.DTOs;
using PayspaceTaxCalculator.Domain.Interfaces;

namespace PayspaceTaxCalculator.Tests
{
    public class Tests
    {
        IConfiguration configuration;
        [SetUp]
        public void Setup()
        {
            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new List<KeyValuePair<string, string?>>
                {
                    new KeyValuePair<string, string?>("FlatValuePerYear","10000"),
                    new KeyValuePair<string, string?>("FlatValuePerYearBelowTwoHundredThousand","5"),
                    new KeyValuePair<string, string?>("FlatRate","17,5"),
                }).Build();
        }

        [Test]
        public void CanValidatePostalCodeTaxCalculationTypeCreate()
        {
            PayspaceResponse<PostalCodeTaxCalculationType> response = PostalCodeTaxCalculationType.Create("", TaxCalculationType.Progressive);
            Assert.False(response.Success);
        }
        [Test]
        public async Task CanCalculateFlatValueBelow200k()
        {
            PayspaceResponse<TaxCalculator> taxCalculatorResponse = await TaxCalculator.CalculateFlatValueTax("A100", 150000, configuration);
            Assert.True(taxCalculatorResponse.Success);
            Assert.True((taxCalculatorResponse.Response)?.CalculatedTaxAmount == 7500);
        }
        [Test]
        public async Task CanCalculateFlatValueAbove200k()
        {
            PayspaceResponse<TaxCalculator> taxCalculatorResponse = await TaxCalculator.CalculateFlatValueTax("A100", 200000, configuration);
            Assert.True(taxCalculatorResponse.Success);
            Assert.True((taxCalculatorResponse.Response)?.CalculatedTaxAmount == 10000);
        }
        [Test]
        public async Task CanCalculateFlatRate()
        {
            PayspaceResponse<TaxCalculator> taxCalculatorResponse = await TaxCalculator.CalculateFlatRateTax("7000", 1200000, configuration);
            Assert.True(taxCalculatorResponse.Success);
            Assert.True((taxCalculatorResponse.Response)?.CalculatedTaxAmount == 210000);
        }
        [Test]
        [TestCase(5000, 500)]
        [TestCase(12000, 1382.5)]
        [TestCase(33951, 4675.15)]
        [TestCase(50000, 7852.5)]
        [TestCase(120000, 25352.5)]
        [TestCase(500000, 141811)]
        [TestCase(2000000, 663430)]
        public async Task CanCalculateProgressiveTaxRate(decimal annualIncome, decimal expectedTaxAmount)
        {
            PayspaceResponse<List<ProgressiveTaxRateDTO>> mockRates = new PayspaceResponse<List<ProgressiveTaxRateDTO>>
            {
                Id = Guid.NewGuid(),
                Success = true,
                Response = new List<ProgressiveTaxRateDTO>
                {
                    new ProgressiveTaxRateDTO { Id = Guid.NewGuid(), From = 0, To = 8350, Rate = 10},
                    new ProgressiveTaxRateDTO { Id = Guid.NewGuid(), From = 8351, To = 33950, Rate = 15},
                    new ProgressiveTaxRateDTO { Id = Guid.NewGuid(), From = 33951, To = 82250, Rate = 25},
                    new ProgressiveTaxRateDTO { Id = Guid.NewGuid(), From = 82251, To = 171550, Rate = 28},
                    new ProgressiveTaxRateDTO { Id = Guid.NewGuid(), From = 171551, To = 372950, Rate = 33},
                    new ProgressiveTaxRateDTO { Id = Guid.NewGuid(), From = 372951, To = -1, Rate = 35}
                }
            };
            Mock<IRepository> repo = new Mock<IRepository>();
            repo.Setup(r => r.GetProgressiveTaxRates());
            //.Returns(Task.FromResult(mockRates)); could not get this to work, opted for just passing in the mockrates instead
            GetProgressiveTaxRatesHandler getProgressiveTaxRatesHandler = new GetProgressiveTaxRatesHandler(repo.Object);
            //PayspaceResponse<List<ProgressiveTaxRateDTO>> ratesResponse = await getProgressiveTaxRatesHandler.Handle(new Application.Queries.GetProgressiveTaxRatesQuery(), new CancellationToken());
            PayspaceResponse<TaxCalculator> taxCalculatorResponse = await TaxCalculator.CalculateProgressiveRateTax("7000", annualIncome, mockRates.Response, configuration);
            Assert.True(taxCalculatorResponse.Success);
            Assert.True((taxCalculatorResponse.Response)?.CalculatedTaxAmount == expectedTaxAmount);
        }
    }
}