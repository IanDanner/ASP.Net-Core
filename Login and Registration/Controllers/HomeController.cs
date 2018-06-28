using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DbConnection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Login_and_Registration.Models;
using Newtonsoft.Json;

namespace Login_and_Registration.Controllers
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

        [Route("register")]
        public IActionResult Register(ValidUserRegisterModel user)
        {
            if(ModelState.IsValid)
            {
                List<Dictionary<string, object>> User = DbConnector.Query($"SELECT * FROM users WHERE email = '{user.Email}'");
                if(User.Count > 0){
                    TempData["Rerror"] = "Email already registered";
                    return View("Index",user);
                }
                DbConnector.Query($"INSERT INTO users (firstName, lastName, email, password, created_at, updated_at) VALUES ('{user.FirstName}','{user.LastName}','{user.Email}','{user.Password}', NOW(), NOW())");
                HttpContext.Session.SetObjectAsJson("Logged", User);
                return RedirectToAction("Success");
            }
            return View("Index",user);
        }

        [HttpGet]
        [HttpPost]
        [Route("login")]
        public IActionResult Login(string email, string password)
        {
            if(email == null){
                TempData["error"] = "No email input";
                return RedirectToAction("Index");
            }
            List<Dictionary<string, object>> User = DbConnector.Query($"SELECT * FROM users WHERE email = '{email}'");
            if(User.Count == 0){
                TempData["error"] = "Email not registered";
                return RedirectToAction("Index");
            }
            foreach(var me in User){
                if((string)me["password"] != password){
                    TempData["error"] = "Password is Incorrect";
                    return RedirectToAction("Index");
                }
            }
            HttpContext.Session.SetObjectAsJson("Logged", User);
            return RedirectToAction("Success");
        }

        [HttpGet]
        [Route("success")]
        public IActionResult Success()
        {   
            TempData["User"] = HttpContext.Session.GetObjectFromJson<List<Dictionary<string, object>>>("Logged");
            return View("Success");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {   
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
    public static class SessionExtensions
    {
        // We can call ".SetObjectAsJson" just like our other session set methods, by passing a key and a value
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            // This helper function simply serializes theobject to JSON and stores it as a string in session
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        
        // generic type T is a stand-in indicating that we need to specify the type on retrieval
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            string value = session.GetString(key);
            // Upon retrieval the object is deserialized based on the type we specified
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
