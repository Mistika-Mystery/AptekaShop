using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using apteka.Data;
using apteka.Models;

namespace apteka.Controllers
{
    public class AkqiasController : Controller
    {
        private readonly ApplicationDbContext2 _context;

        public AkqiasController(ApplicationDbContext2 context)
        {
            _context = context;
        }

        // GET: Akqias
        public async Task<IActionResult> Index()
        {
            return View(await _context.Akqias.ToListAsync());
        }

        // GET: Akqias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akqia = await _context.Akqias
                .FirstOrDefaultAsync(m => m.IDAk == id);
            if (akqia == null)
            {
                return NotFound();
            }

            return View(akqia);
        }

        // GET: Akqias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Akqias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDAk,Naz,Pr")] Akqia akqia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(akqia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(akqia);
        }

        // GET: Akqias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akqia = await _context.Akqias.FindAsync(id);
            if (akqia == null)
            {
                return NotFound();
            }
            return View(akqia);
        }

        // POST: Akqias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDAk,Naz,Pr")] Akqia akqia)
        {
            if (id != akqia.IDAk)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(akqia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkqiaExists(akqia.IDAk))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(akqia);
        }

        // GET: Akqias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akqia = await _context.Akqias
                .FirstOrDefaultAsync(m => m.IDAk == id);
            if (akqia == null)
            {
                return NotFound();
            }

            return View(akqia);
        }

        // POST: Akqias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akqia = await _context.Akqias.FindAsync(id);
            if (akqia != null)
            {
                _context.Akqias.Remove(akqia);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkqiaExists(int id)
        {
            return _context.Akqias.Any(e => e.IDAk == id);
        }
    }
}
