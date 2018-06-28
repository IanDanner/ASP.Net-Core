using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PokeInfo.Models;

namespace PokeInfo.Controllers
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
        [Route("pokeinfo")]
        public IActionResult QueryPoke(int pokeId)
        {
            var PokeInfo = new Dictionary<string, object>();
            WebRequest.GetPokemonDataAsync(pokeId, ApiResponse =>
            {
                PokeInfo = ApiResponse;
            }
            ).Wait();
            ViewBag.info = PokeInfo;
            return View("Result");
        }
    }
}
