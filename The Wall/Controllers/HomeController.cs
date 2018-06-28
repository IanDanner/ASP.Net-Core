using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DbConnection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using The_Wall.Models;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace The_Wall.Controllers
{
    public class LoginRegController : Controller
    {
        [Route("/")]
        public IActionResult Index() => View();

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
                string hashed = HashedPass(user.Password);
                DbConnector.Query($"INSERT INTO users (firstName, lastName, email, password, created_at, updated_at) VALUES ('{user.FirstName}','{user.LastName}','{user.Email}','{HashedPass(user.Password)}', NOW(), NOW())");
                User = DbConnector.Query($"SELECT * FROM users WHERE email = '{user.Email}'");
                HttpContext.Session.SetObjectAsJson("Logged", User);
                return RedirectToAction("The_Wall", "TheWall");
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
            foreach(var me in User) //TESTING LOGIN PASSWORD TO DATABASE PASSWORD
            {
                /* Fetch the stored value */
                string savedPasswordHash = (string)me["password"];
                /* Extract the bytes */
                byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
                /* Get the salt */
                byte[] salt = new byte[16];
                Array.Copy(hashBytes, 0, salt, 0, 16);
                /* Compute the hash on the password the user entered */
                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
                byte[] hash = pbkdf2.GetBytes(20);
                /* Compare the results */
                for (int i=0; i < 20; i++)
                {
                    if (hashBytes[i+16] != hash[i])
                    {
                        TempData["error"] = "Password is Incorrect";
                        return RedirectToAction("Index");
                    }
                }
            }
            HttpContext.Session.SetObjectAsJson("Logged", User);
            return RedirectToAction("The_Wall", "TheWall");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {   
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public string HashedPass(string password){ //HASHING PASSWORD FOR DATABASE
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;
        }
    }
    public static class SessionExtensions
    {
        // We can call ".SetObjectAsJson" just like our other session set methods, by passing a key and a value
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            string value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
