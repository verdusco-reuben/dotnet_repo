using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using beltexam.Models;
using Microsoft.EntityFrameworkCore;

namespace beltexam.Controllers
{
    public class HomeController : Controller
    {
        private SomeContext _context;
 
        public HomeController(SomeContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("submit")]
        public IActionResult Register(UserViewModel guy)
        {
            //checks if form is valid and email not in database
            User possible_user = _context.users.SingleOrDefault(user => user.EmailAddress == guy.EmailAddress);
            if(possible_user != null)
            {
                return View("index");
            }
            if (ModelState.IsValid)
            {
                User newUser = new User
                {
                    FirstName = guy.FirstName,
                    LastName = guy.LastName,
                    EmailAddress = guy.EmailAddress,
                    Password = guy.Password
                };
                _context.users.Add(newUser);
                _context.SaveChanges();
                HttpContext.Session.SetString("name", newUser.FirstName);
                HttpContext.Session.SetInt32("id", newUser.UserId);
                return RedirectToAction("dashboard", "Dashboard");
            }
            //else
            return View("index", guy);
        }

        [HttpPost]
        [Route("process")]
        public IActionResult LogIn(string EmailAddress, string Password)
        { 
            User possible_user = _context.users.SingleOrDefault(user => user.EmailAddress == EmailAddress && user.Password == Password);
            if(possible_user == null)
            {
                ViewBag.errors = "Password is invalid";
                return View("Index");
            }
            HttpContext.Session.SetString("name", possible_user.FirstName);
            HttpContext.Session.SetInt32("id", possible_user.UserId);
            return RedirectToAction("dashboard", "Dashboard");
        }
    }
}
