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

        [HttpGet]
        public IActionResult Prihlaseni()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Prihlaseni(string username,
            string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewData["chyba"] = "Jméno nebo heslo není zadáno.";

                return View();
            }

            User? prihlasenyUzivatel = _staff_info
                .Users
                .Where(u => u.Username == username)
                .FirstOrDefault();

            if (prihlasenyUzivatel == null)
            {
                ViewData["chyba"] = "Neznámý uživatel.";

                return View();
            }

            if (prihlasenyUzivatel.Password != password)
            {
                ViewData["chyba"] = "Nesprávné heslo.";
                return View();
            }

            return Redirect("/User/" + (prihlasenyUzivatel.Role == "ucitel" ? "UcitelProfil" : "ZakProfil"));

        }

        public IActionResult UcitelProfil()
        {
            return View();
        }


        public IActionResult ZakProfil()
        {
            return View();
        }
    }
}

