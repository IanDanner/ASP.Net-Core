using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quoting_Dojo.Models;
using DbConnection;

namespace Quoting_Dojo.Controllers
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
        [Route("quotes")]
        public IActionResult Quotes(string name, string quote)
        {
            if(name != null || quote != null){
                DbConnector.Query($"INSERT INTO quotes (author, quote, created_at, updated_at) VALUES ('{name}','{quote}', NOW(), NOW())");
            }
            List<Dictionary<string, object>> AllQuotes = DbConnector.Query("SELECT * FROM quotes");
            ViewBag.quotes = AllQuotes;
            return View();
        }


    }
}
