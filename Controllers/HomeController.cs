using System;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DriveIn.Models;
using BCrypt.Net;

namespace DriveIn.Controllers;

public class HomeController : Controller
{
    private readonly DatabaseContext _ctx;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, DatabaseContext ctx)
    {
        _ctx = ctx;
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Index(LoginRegister model, string button, User user, Profile profile)
    {
        if (button == "login")
        {
            try
            {
                var getUser = _ctx.User.FirstOrDefault(u => u.Email == model.Email);
                if (getUser != null && VerifyPassword(model.Password, getUser.PasswordHash))
                {
                    // HttpContext.Session.SetInt32("authorizedId", getUser.Id);
                    return RedirectToAction("Profile", "Profile", new { id = getUser.Id });
                }
                TempData["msg"] = "Login Failed!";
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["msg"] = "Login Failed!";
                return RedirectToAction("Index");
            }
        }
        else if (button == "register")
        {
            try
            {
                if (!EmailExists(model.Email))
                {
                    user.Email = model.Email;
                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
                    user.PasswordHash = passwordHash;
                    profile.Name = model.Name;
                    profile.User = user;
                    _ctx.Add(user);
                    _ctx.Add(profile);
                    _ctx.SaveChanges();
                    TempData["msg"]="User Registered!";
                    return RedirectToAction("Index");
                }
                TempData["msg"]="Email Already Exists!";
                return RedirectToAction("Index");
            }
            catch (System.Exception)
            {
                TempData["msg"]="Registration Failed!";
                return RedirectToAction("Index");
            }
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.message = TempData["msg"] as string;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private bool EmailExists(string email)
    {
        var user = _ctx.User.FirstOrDefault(u => u.Email == email);
        return user != null;
    }
    private static bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
