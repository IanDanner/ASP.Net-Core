using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
 
namespace Dojodachi.Controllers
{
    public class DojodachiController : Controller
    {
        public static Kirbydachi myKirbydachi;
        public static Random rand = new Random();

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if (myKirbydachi == null){
                myKirbydachi = new Kirbydachi();
                ViewBag.Message = "Greetings!";
            }
            else{
                ViewBag.Message = TempData["message"];
            }
            // if (myKirbydachi.Happy){
            //     ViewBag.Img = "/imgs/happy.png";
            // }
            // else{
            //     ViewBag.Img = "imgs/nothappy.png";
            // }
            if(!myKirbydachi.Alive && myKirbydachi.Win){
                ViewBag.Message = "Congratulations! You Win!";
            }
            else if(!myKirbydachi.Alive && !myKirbydachi.Win){
                ViewBag.Img = "";
                ViewBag.Message = "Your Kirbydachi has Died!";
            }
            ViewBag.Image = myKirbydachi.Image;
            ViewBag.Happiness = myKirbydachi.Happiness;
            ViewBag.Fullness = myKirbydachi.Fullness;
            ViewBag.Energy = myKirbydachi.Energy;
            ViewBag.Meals = myKirbydachi.Meals;
            ViewBag.Alive = myKirbydachi.Alive;
            return View();
        }
        [HttpGet]
        [Route("feed")]
        public IActionResult Feed()
        {
            if (!myKirbydachi.Alive){
                return RedirectToAction("Index");
            }
            if (myKirbydachi.Meals <= 0){
                TempData["message"] = "You Have no Meals!!!";
                return RedirectToAction("Index");
            }
            if(myKirbydachi.Happy){
                int newFullness = rand.Next(5,10);
                myKirbydachi.Fullness += newFullness;
                TempData["message"] = $"You Feed your Kirbydachi a Meal. Fullness +{newFullness}, Meals -1";
                myKirbydachi.Image = "/imgs/eating.png";                            
            }
            else{
                TempData["message"] = "Your Kirbydachi does not like the Meal. Fullness +0, Meals -1";
                myKirbydachi.Image = "/imgs/notHappy.png";                          
            }
            myKirbydachi.Meals -= 1;
            myKirbydachi.DeathCheck();            
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("play")]
        public IActionResult Play()
        {            
            if (!myKirbydachi.Alive){
                return RedirectToAction("Index");
            }
            if (myKirbydachi.Energy <= 0){
                TempData["message"] = "You Have no Energy!!!";
                return RedirectToAction("Index");
            }
            if(myKirbydachi.Happy){
                int newHappiness = rand.Next(5,10);
                myKirbydachi.Happiness += newHappiness;
                TempData["message"] = $"You play with your Kirbydachi. Happiness +{newHappiness}, Meals -1"; 
                myKirbydachi.Image = "/imgs/play.png";                                                     
            }
            else{
                TempData["message"] = "Your Kirbydachi want to play. Happiness +0, Meals -1";   
                myKirbydachi.Image = "/imgs/notHappy.png";                                                                   
            }
            myKirbydachi.Energy -= 5;
            myKirbydachi.DeathCheck();            
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("work")]
        public IActionResult Work()
        {
            if (!myKirbydachi.Alive){
                return RedirectToAction("Index");
            }
            if (myKirbydachi.Energy <= 0){
                TempData["message"] = "You Have no Energy!!!";
                return RedirectToAction("Index");
            }
            myKirbydachi.Image = "/imgs/work.png";                          
            int newMeals = rand.Next(1,4);
            myKirbydachi.Meals += newMeals;
            TempData["message"] = $"You went to Work to get some Meals. Meals +{newMeals}, Energy -5";            
            myKirbydachi.Energy -= 5;
            myKirbydachi.DeathCheck();            
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("sleep")]
        public IActionResult Sleep()
        {
            if (!myKirbydachi.Alive){
                return RedirectToAction("Index");
            }
            myKirbydachi.Energy += 15;
            myKirbydachi.Fullness -= 5;
            myKirbydachi.Happiness -= 5;
            TempData["message"] = "Your Kirbydachi goes to Sleep. Energy +15, Fullness -5, Happiness -5";
            myKirbydachi.Image = "/imgs/sleeping.png";                                                              
            myKirbydachi.DeathCheck();            
            myKirbydachi.Happy = true;                                    
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("restart")]
        public IActionResult Reset()
        {
            myKirbydachi = null;    
            return RedirectToAction("Index");
        }

        public class Kirbydachi{
            public static Random rand = new Random();
            public int Fullness {get;set;}
            public int Happiness {get;set;}
            public int Meals {get;set;}
            public int Energy {get;set;}
            public bool Alive {get;set;}
            public bool Happy {get;set;}
            public bool Win {get;set;}
            public string Image {get;set;}
            

            public Kirbydachi(){
                Fullness = 20;
                Happiness = 20;
                Meals = 3;
                Energy = 50;
                Alive = true;
                Happy = true;
                Win = false;
                Image = "/imgs/happy.png";
            }
            public void DeathCheck(){
                if (Happiness <= 0 || Fullness <= 0){
                    Alive = false;
                    myKirbydachi.Image = "/imgs/death.png";                                                  
                }
                else if(Energy >= 100 && Fullness >= 100 && Happiness >= 100){
                    Alive = false;
                    Win = true;
                    myKirbydachi.Image = "/imgs/win.png";                                                                      
                }
            }
            public void HappyCheck(){
                int num = rand.Next(1,5); 
                if (num == 3){
                    Happy = false;
                    Image = "/imgs/notHappy.png";
                }
                else{
                    Happy = true;
                    Image = "/imgs/happy.png";
                }
            }
        }
    }
}
