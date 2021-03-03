using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using hobbie.Models;
using hobbie.Repositories;
using hobbie.Utilis;

namespace hobbie.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserRepository repository;
        private Log log = Log.getInstance();


        public HomeController(IUserRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await repository.getAllUsers());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                var user = await repository.findUserId(id);
                if (user == null) throw new Exception("User is null");
                return View(user);
            }
            catch (Exception ex)
            {
                log.error("User edit get error id : {0}", ex: ex, id);
                return View();
            }
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
    }
}
