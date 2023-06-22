using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PayspaceTaxCalculator.Controllers
{
    //[Authorize(Policy = "Admin")] //TODO
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
