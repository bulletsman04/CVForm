﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CommunityCertForT;
using CommunityCertForT.Helpers;
using Microsoft.AspNetCore.Mvc;
using CVForm.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace CVForm.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _configuration;
        private AppSettings AppSettings { get; set; }
        public HomeController(IConfiguration Configuration)
        {
            _configuration = Configuration;
            AppSettings = _configuration.GetSection("AppSettings").Get<AppSettings>();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        //[Authorize]
        public async Task<IActionResult> Contact()
        {
            AADGraph graph = new AADGraph(AppSettings);
            string groupName = "Admins";
            string groupId = AppSettings.AADGroups.FirstOrDefault(g =>
                String.Compare(g.Name, groupName) == 0).Id;
            bool isIngroup = await graph.IsUserInGroup(User.Claims, groupId);

            if (!isIngroup)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewData["Message"] = "Your contact page.";
             
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
    }
}
