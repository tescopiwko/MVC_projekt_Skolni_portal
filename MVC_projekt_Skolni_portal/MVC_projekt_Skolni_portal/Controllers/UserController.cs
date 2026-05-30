using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;


namespace _06_AspNetCore.Controllers
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
    }
}
