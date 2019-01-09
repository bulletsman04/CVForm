﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CVForm.EntityFramework;
using CVForm.Models;
using Microsoft.AspNetCore.Mvc;

namespace CVForm.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly DataContext _context;

        public CompaniesController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Companies.ToList());
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var offer = _context.Companies.FirstOrDefault(item => item.ID == id);

            if (offer == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(offer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Company model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var offer = _context.Companies.FirstOrDefault(item => item.ID == model.ID);
            offer.Name = model.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Company model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _context.Companies.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            _context.Companies.RemoveRange(_context.Companies.Where(item => item.ID == id));
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}