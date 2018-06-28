using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Form_Submission.Models;

namespace Form_Submission.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [HttpPost]
        [Route("register")]
        public IActionResult Register(string firstName, string lastName, int age, string email, string password){
            ValidFormModel NewUser = new ValidFormModel
            {
                FirstName = firstName,
                LastName = lastName,
                Age = age,
                Email = email,
                Password = password
            };

            if(!TryValidateModel(NewUser)){
                ViewBag.errors = ModelState.Values;
                return View("Errors");
            }
            return View("Success"); 
        }
    }
}
