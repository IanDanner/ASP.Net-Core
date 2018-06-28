using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
 
namespace Calling_Card.Controllers
{
    public class Calling_CardController : Controller
    {
        // [HttpGet]
        // [Route("")]
        // public IActionResult Other()
        // {
        //     return View("Hello");
        // }

        [HttpGet]
        [Route("{FirstName}/{LastName}/{Age}/{FavoriteColor}")]
        public string Method(string FirstName,string LastName,int Age,string FavoriteColor)
        {
            return $"First Name: {FirstName} | Last Name: {LastName} | Age: {Age} | Favorite Color: {FavoriteColor}";
        }
    }
}
