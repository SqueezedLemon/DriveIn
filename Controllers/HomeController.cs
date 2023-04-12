using System;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DriveIn.Models;

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
    public IActionResult Index(LoginRegister model, string button, User user)
    {
        if (button == "login")
        {}
        else if (button == "register")
        {
            try
            {
                user.Email = model.Email;
                user.Name = model.Name;
                byte[] passwordSalt = GeneratePasswordSalt();
                byte[] passwordHash = HashPassword(model.Password, passwordSalt);
                user.PasswordSalt = passwordSalt;
                user.PasswordHash = passwordHash;
                _ctx.Add(user);
                _ctx.SaveChanges();
                TempData["msg"]="User Registered!";
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

    private static byte[] GeneratePasswordSalt()
    {
        byte[] salt = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }

    private static byte[] HashPassword (string password, byte[] salt)
    {
        var hmac = new HMACSHA512(salt);
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return hash;
    }
}
