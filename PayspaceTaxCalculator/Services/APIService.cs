using MediatR;
using Newtonsoft.Json;
using PayspaceTaxCalculator.API.Models;
using PayspaceTaxCalculator.Domain.DTOs;
using System.Text;

namespace PayspaceTaxCalculator.Services
{
    public class APIService : IAPIService
    {
        private readonly IConfiguration configuration;

        public APIService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<PayspaceResponse<TaxCalculatorDTO>> CalculateTax(string postalCode, decimal annualIncome)
        {
            using HttpClient client = new HttpClient();
            if (configuration.GetSection("APIBaseAddress").Value == null)
                return await Task.FromResult(new PayspaceResponse<TaxCalculatorDTO> { Success = false, ErrorMessage = "Base Address has not been configured." });
            client.BaseAddress = new Uri(configuration.GetSection("APIBaseAddress").Value);
            HttpRequestMessage request = new HttpRequestMessage();
            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri($"{client.BaseAddress}api/TaxCalculation");
            request.Content = new StringContent(JsonConvert.SerializeObject(new TaxCalculationInput { PostalCode = postalCode, AnnualIncome = annualIncome }), Encoding.UTF8, "application/json");
            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return new PayspaceResponse<TaxCalculatorDTO> { Success = false, ErrorMessage = result };
            var calculation = JsonConvert.DeserializeObject<PayspaceResponse<TaxCalculatorDTO>>(result);
            return calculation;
        }
    }
}
