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

                ViewData["chybaTrida"] = "Pro registraci žáka musíte vybrat tridu.";
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
            HttpContext.Session.SetString("prihlaseny", username);

            return Redirect("/User/" + (prihlasenyUzivatel.Role == "ucitel" ? "UcitelProfil" : "ZakProfil"));

        }

        public IActionResult UcitelProfil(string? vybranaTrida, string? aktivniZalozka)
        {
            string? prihlaseny = HttpContext.Session.GetString("prihlaseny");

            if (prihlaseny == null)
            {
                return Redirect("/User/Prihlaseni"); 
            }

            
            User prihlasenyUzivatel = _staff_info.Users
                .Where(u => u.Username == prihlaseny)
                .FirstOrDefault();

            // 3. Bezpeènostní pojistka: Co když žák ruènì pøepsal URL adresu v prohlížeèi na /User/UcitelProfil?
            // Musíme ovìøit, že ten, kdo je v Session, je opravdu uèitel.
            if (prihlasenyUzivatel == null || prihlasenyUzivatel.Role != "ucitel")
            {
                return Redirect("/User/Prihlaseni");
            }

            ViewData["ZvolenaTrida"] = vybranaTrida;

            ViewData["AktivniZalozka"] = string.IsNullOrEmpty(aktivniZalozka) ? "profil" : aktivniZalozka;


            List<User> zaci = new List<User>();
            if (!string.IsNullOrEmpty(vybranaTrida))
            {
                zaci = _staff_info.Users
                    .Where(u => u.Role == "zak" && u.Trida == vybranaTrida)
                    .ToList();
            }

            ViewBag.Zaci = zaci;


            return View(prihlasenyUzivatel);
        }

        [HttpPost]
        public IActionResult ZapisZnamku(string ClassName, string StudentName, int Value, int Weight, string Description)
        {
            // 1. Kontrola, zda je uživatel vůbec přihlášený (bezpečnost)
            string? prihlaseny = HttpContext.Session.GetString("prihlaseny");
            if (prihlaseny == null)
            {
                return Redirect("/User/Prihlaseni");
            }

            // 2. Vytvoření nového objektu známky pro databázi
            // Názvy vlastností (JmenoZaka, Hodnota...) uprav podle toho, jak přesně vypadá tvůj model Grade
            Grade novaZnamka = new Grade()
            {
                ClassName = ClassName,          // Přijde ze skrytého inputu <input type="hidden" name="ClassName" ... />
                StudentName = StudentName,    // Přijde z roletky <select name="StudentName"> ("Jméno Příjmení")
                Value = Value,            // Přijde z <select name="Value">
                Weight = Weight,              // Přijde z <select name="Weight">
                Description = Description,        // Přijde z <input name="Description" />
                DateCreated = DateTime.Now  // Automaticky uložíme aktuální čas zápisu
            };

            // 3. Uložení do databáze Grades
            _staff_info.Grades.Add(novaZnamka); // Pokud se tvůj DbSet jmenuje jinak (např. Znamky), oprav název
            _staff_info.SaveChanges();

            // 4. WORKFLOW PŘESMĚROVÁNÍ (To nejdůležitější bez JS):
            // Přesměrujeme učitele zpět na profil, ale do URL mu podstrčíme parametry,
            // které si Model Binder při novém načítání přečte (vybranaTrida a aktivniZalozka).
            // Křížek #hodnoceni-sekce zajistí, že prohlížeč po reloadu ihned sjede dolů na známky.
            return Redirect($"/User/UcitelProfil?vybranaTrida={ClassName}&aktivniZalozka=znamky#hodnoceni-sekce");
        }


        public IActionResult ZakProfil()
        {
            
            string? prihlaseny = HttpContext.Session.GetString("prihlaseny");

            if (prihlaseny == null)
            {
                return Redirect("/User/Prihlaseni");
            }

            
            User prihlasenyUzivatel = _staff_info.Users
                .Where(u => u.Username == prihlaseny)
                .FirstOrDefault();

            // 3. Pojistka: Pokud by se sem pokusil vlézt uèitel pøes URL, vyhodíme ho
            if (prihlasenyUzivatel == null || prihlasenyUzivatel.Role != "zak")
            {
                return Redirect("/User/Prihlaseni");
            }

            
            return View(prihlasenyUzivatel);
        }

        public IActionResult Odhlaseni()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete(".AspNetCore.Session");

            return Redirect("/Home/Index");
        }
    }
}

