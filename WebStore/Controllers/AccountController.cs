using WebStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System;
using Microsoft.AspNetCore.Http;

namespace WebStore.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IAccountService accountService, IStoreService storeService)
           : base(accountService, storeService)
        {
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        /// <summary>
        /// Bejelentkezés.
        /// </summary>
        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        /// <summary>
        /// Bejelentkezés.
        /// </summary>
        /// <param name="user">A bejelentkezés adatai.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel user)
        {
            if (!ModelState.IsValid)
                return View("Login", user);

            // bejelentkeztetjük a felhasználót
            if (!accountService.Login(user))
            {
                // nem szeretnénk, ha a felhasználó tudná, hogy a felhasználónévvel, vagy a jelszóval van-e baj, így csak általános hibát jelzünk
                ModelState.AddModelError("", "Hibás felhasználónév, vagy jelszó.");
                return View("Login", user);
            }

            // ha sikeres volt az ellenőrzés
            HttpContext.Session.SetString("user", user.UserName); // felvesszük a felhasználó nevét a munkamenetbe
            HttpContext.Session.Set("shoppingCart", new ShoppingCart(storeService)); // felvesszünk egy új kosarat
            return RedirectToAction("Index", "Home"); // átirányítjuk a főoldalra
        }

        /// <summary>
        /// Regisztráció.
        /// </summary>
        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        /// <summary>
        /// Regisztráció.
        /// </summary>
        /// <param name="customer">Regisztrációs adatok.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegistrationViewModel customer)
        {
            // végrehajtjuk az ellenőrzéseket
            if (!ModelState.IsValid)
                return View("Register", customer);

            if (!accountService.Register(customer))
            {
                ModelState.AddModelError("UserName", "A megadott felhasználónév már létezik.");
                return View("Register", customer);
            }

            ViewBag.Information = "A regisztráció sikeres volt. Kérjük, jelentkezzen be.";

            if (HttpContext.Session.GetString("user") != null) // ha be volt jelentkezve egy felhasználó, akkor kijelentkeztetjük
                HttpContext.Session.Remove("user");

            return RedirectToAction("Login");
        }

        /// <summary>
        /// Kijelentkezés.
        /// </summary>
        public IActionResult Logout()
        {
            accountService.Logout();

            return RedirectToAction("Index", "Home"); // átirányítjuk a főoldalra
        }


    }
}
