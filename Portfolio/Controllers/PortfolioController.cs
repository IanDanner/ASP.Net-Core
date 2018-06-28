using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
 
namespace Portfolio.Controllers
{
    public class PortfolioController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Home()
        {
            return View("Portfolio");
        }

        [HttpGet]
        [Route("projects")]
        public IActionResult Projects()
        {
            return View("Projects");
        }

        [HttpGet]
        [Route("contact")]
        public IActionResult Contact()
        {
            return View("Contact");
        }
    }
}
