using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
 
namespace Dojo_Survey.Controllers
{
    public class Dojo_SurveyController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View("Dojo_Survey");
        }

        [HttpPost]
        [Route("result")]
        public IActionResult Submit(string  username, string location, string language, string comments)
        {
            ViewBag.username = username;
            ViewBag.location = location; 
            ViewBag.language = language; 
            ViewBag.comments = comments;
            return View("Result");
        }
    }
}
