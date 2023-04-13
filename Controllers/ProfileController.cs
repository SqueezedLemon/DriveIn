using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DriveIn.Controllers
{
    public class ProfileController : Controller
    {
        private readonly DatabaseContext _ctx;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(ILogger<ProfileController> logger, DatabaseContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        [HttpGet("{id}")]
        public IActionResult Profile(int id)

        {
            var getProfile = _ctx.Profile.FirstOrDefault(u => u.UserId == id);
            return View(getProfile);
        }

        [HttpPut]
        public IActionResult Profile(Profile profile)

        {
                var getProfile = _ctx.Profile.FirstOrDefault(p => p.Id == profile.Id);
                if (getProfile != null)
                {
                    getProfile.Name = profile.Name;
                    getProfile.Address = profile.Address;
                    getProfile.Phone = profile.Phone;
                    getProfile.LicenceNumber = profile.LicenceNumber;
                    getProfile.Issue = profile.Issue;
                    _ctx.SaveChanges();
                    return RedirectToAction("Profile", new { id = profile.UserId });
                }
                return RedirectToAction("Index","Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}