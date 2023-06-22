using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        public async Task CanCreateAndSavePostalCodeTaxCalculationTypes()
        {
            CreatePostalCodeTaxCalculationTypeHandler createPostalCodeTaxCalculationTypeHandler = new CreatePostalCodeTaxCalculationTypeHandler(repo);
            var result = await createPostalCodeTaxCalculationTypeHandler.Handle(new CreatePostalCodeTaxCalculationTypeCommand((new Random().Next(1000, 5000)).ToString(), Domain.TaxCalculationType.Progressive),
                new CancellationToken());
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
    }
}
