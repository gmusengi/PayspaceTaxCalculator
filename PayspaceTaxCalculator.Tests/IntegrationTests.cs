using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using PayspaceTaxCalculator.Application.Command;
using PayspaceTaxCalculator.Application.Handlers;
using PayspaceTaxCalculator.Domain;
using PayspaceTaxCalculator.Domain.DTOs;
using PayspaceTaxCalculator.Domain.Interfaces;
using PayspaceTaxCalculator.Infrastructure;

namespace PayspaceTaxCalculator.Tests
{
    public class IntegrationTests
    {
        IConfiguration configuration;
        IRepository repo;
        [SetUp]
        public void Setup()
        {
            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new List<KeyValuePair<string, string?>>
                {
                    new KeyValuePair<string, string?>("ConnectionStrings:DefaultConnection","Server=(localdb)\\MSSQLLocalDB;Database=PayspaceTaxCalculator;Trusted_Connection=True;"),
                    new KeyValuePair<string, string?>("FlatValuePerYear","10000"),
                    new KeyValuePair<string, string?>("FlatValuePerYearBelowTwoHundredThousand","5"),
                    new KeyValuePair<string, string?>("FlatRate","17,5"),
                }).Build();
            DbContextOptions<PayspaceDbContext> options = new DbContextOptionsBuilder<PayspaceDbContext>()
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .Options;
            PayspaceDbContext payspaceDbContext = new PayspaceDbContext(options);
            repo = new Repository(payspaceDbContext, configuration);
        }

        [Test]
        [TestCase("7441", TaxCalculationType.Progressive)]
        [TestCase("A100", TaxCalculationType.FlatValue)]
        [TestCase("7000", TaxCalculationType.FlatRate)]
        [TestCase("1000", TaxCalculationType.Progressive)]
        public async Task CanCreateAndSavePostalCodeTaxCalculationTypes(string postalCode, TaxCalculationType taxCalculationType)
        {
            CreatePostalCodeTaxCalculationTypeHandler createPostalCodeTaxCalculationTypeHandler = new CreatePostalCodeTaxCalculationTypeHandler(repo);
            var result = await createPostalCodeTaxCalculationTypeHandler.Handle(new CreatePostalCodeTaxCalculationTypeCommand(postalCode, taxCalculationType),
                new CancellationToken());
            Assert.True(result.Success);
        }
        [Test]
        public async Task CanGetTaxCalculationTypeFromPostalCode()
        {
            GetTaxCalculationTypeFromPostalCodeHandler calculationTypeFromPostalCodeHandler = new GetTaxCalculationTypeFromPostalCodeHandler(repo);
            var result = await calculationTypeFromPostalCodeHandler.Handle(new Application.Queries.GetTaxCalculationTypeFromPostalCodeQuery("7441"), new CancellationToken());
            Assert.True(result.Success);
        }
        [Test]
        public async Task CanCreateAndSaveProgressTaxRates()
        {
            CreateProgressiveTaxRateHandler createProgressiveTaxRateHandler = new CreateProgressiveTaxRateHandler(repo);
            var result = await createProgressiveTaxRateHandler.Handle(new CreateProgressiveTaxRateCommand(10, 0, 8350), new CancellationToken());
            Assert.True(result.Success);
        }
        [Test]
        public async Task CanCalculateAndSaveFlatValueBelow200k()
        {
            CalculateFlatValueTaxHandler calculateFlatValueTaxHandler = new CalculateFlatValueTaxHandler(repo, configuration);
            PayspaceResponse<TaxCalculatorDTO> taxCalculatorResponse = await calculateFlatValueTaxHandler.Handle(new CalculateFlatValueTaxCommand("A100", 150000), new CancellationToken());
            Assert.True(taxCalculatorResponse.Success);
            Assert.True((taxCalculatorResponse.Response)?.CalculatedTaxAmount == 7500);
        }
        [Test]
        public async Task CanCalculateAndSaveFlatValueAbove200k()
        {
            CalculateFlatValueTaxHandler calculateFlatValueTaxHandler = new CalculateFlatValueTaxHandler(repo, configuration);
            PayspaceResponse<TaxCalculatorDTO> taxCalculatorResponse = await calculateFlatValueTaxHandler.Handle(new CalculateFlatValueTaxCommand("A100", 200000), new CancellationToken());
            Assert.True(taxCalculatorResponse.Success);
            Assert.True((taxCalculatorResponse.Response)?.CalculatedTaxAmount == 10000);
        }
        [Test]
        public async Task CanCalculateAndSaveFlatRate()
        {
            CalculateFlatRateTaxHandler calculateFlatRateTaxHandler = new CalculateFlatRateTaxHandler(repo, configuration);
            PayspaceResponse<TaxCalculatorDTO> taxCalculatorResponse = await calculateFlatRateTaxHandler.Handle(new CalculateFlatRateTaxCommand("7000", 1200000), new CancellationToken());
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
        public async Task CanCalculateAndSaveProgressiveTaxRate(decimal annualIncome, decimal expectedTaxAmount)
        {
            GetProgressiveTaxRatesHandler getProgressiveTaxRatesHandler = new GetProgressiveTaxRatesHandler(repo);
            PayspaceResponse<List<ProgressiveTaxRateDTO>> ratesResponse = await getProgressiveTaxRatesHandler.Handle(new Application.Queries.GetProgressiveTaxRatesQuery(), new CancellationToken());
            CalculateProgressiveTaxHandler calculateProgressiveTaxHandler = new CalculateProgressiveTaxHandler(repo);
            PayspaceResponse<TaxCalculatorDTO> taxCalculatorResponse = await calculateProgressiveTaxHandler.Handle(new CalculateProgressiveTaxCommand("7441", annualIncome, ratesResponse.Response), new CancellationToken());
            Assert.True(taxCalculatorResponse.Success);
            Assert.True((taxCalculatorResponse.Response)?.CalculatedTaxAmount == expectedTaxAmount);
        }

    }
}
