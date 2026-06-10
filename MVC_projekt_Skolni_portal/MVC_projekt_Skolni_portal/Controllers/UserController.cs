using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace MVC_projekt_Skolni_portal.Controllers
{
    public class UserController : Controller
    {


        public IActionResult Prihlaseni()
        { 
            return View();
        }


        public IActionResult UcitelProfil()
        {
            return View();
        }


        public IActionResult ZakProfil()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registrace()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrace(string role, string username, string password, string confirmPassword,
            string jmeno, string prijmeni, string adresa, string telefon, string skolniEmail, 
            string trida, string predmet
            )
        {
            if (role == "zak" && string.IsNullOrEmpty(trida))
            {

                ViewData["chybaTrida"] = "Pro registraci žáka musíte vybrat pøedmìt.";
                return View();
            }

            else if (role == "ucitel" && string.IsNullOrEmpty(predmet))
            {
                ViewData["chybaPredmet"] = "Pro registraci uèitele musíte vybrat pøedmìt.";
                return View();
            }
            else if (password != confirmPassword)
            {
                ViewData["chybaHeslo"] = "Hesla se neshodují.";
                return View();
            }
            else
            {
                if (role == "zak")
                {

                }

                else if (role == "ucitel")
                {

                }

                return Redirect("/User/Prihlaseni");
            }
                       
                    

        }
    }
}

