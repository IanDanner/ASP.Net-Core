using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
 
namespace Random_Passcode.Controllers
{
    public class Random_PasscodeController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("count") == null){
                HttpContext.Session.SetInt32("count",1);
            }
            else{
                HttpContext.Session.SetInt32("count",((int)HttpContext.Session.GetInt32("count")+1));
            }
            ViewBag.count = (int)HttpContext.Session.GetInt32("count");
            ViewBag.passcode = Generate();
            return View("Index");
        }
        public string Generate()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[14];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            string passcode = new String(stringChars);

            return passcode;
        }
    }
}
