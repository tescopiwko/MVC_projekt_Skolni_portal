using Microsoft.AspNetCore.Mvc;
using MVC_projekt_Skolni_portal.Data;
using MVC_projekt_Skolni_portal.Models;


namespace MVC_projekt_Skolni_portal.Controllers
{
    public class UserController : Controller
    {
        private readonly KontextDatabaze _staff_info;

        public UserController(KontextDatabaze staff_info)
        {
            _staff_info = staff_info;
        }


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
                User novyUzivatel = new User()
                {
                    Role = role,
                    Username = username,
                    Password = password,
                    Jmeno = jmeno,
                    Prijmeni = prijmeni,
                    Adresa = adresa,
                    Telefon = telefon,
                    SkolniEmail = skolniEmail,

                    Trida = role == "zak" ? trida : null,
                    Predmet = role == "ucitel" ? predmet : null
                };

                _staff_info.Users.Add(novyUzivatel);
                _staff_info.SaveChanges();


                return Redirect("/User/Prihlaseni");
            }
                       
                    

        }
    }
}

