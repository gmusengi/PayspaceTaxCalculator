using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayspaceTaxCalculator.API.Models;
using PayspaceTaxCalculator.Domain.DTOs;
using PayspaceTaxCalculator.Models;
using PayspaceTaxCalculator.Services;
using System.Diagnostics;

namespace PayspaceTaxCalculator.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAPIService apiService;

        public HomeController(ILogger<HomeController> logger, IAPIService apiService)
        {
            _logger = logger;
            this.apiService = apiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index(TaxCalculationInput taxCalculationInput)
        {
            PayspaceResponse<TaxCalculatorDTO> result = await apiService.CalculateTax(taxCalculationInput.PostalCode, taxCalculationInput.AnnualIncome);
            if (!result.Success)
            {
                ModelState.AddModelError("",result.ErrorMessage);
                return View(taxCalculationInput);
            }
            ViewData["CalculationResult"] = result;
            return View(taxCalculationInput);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}