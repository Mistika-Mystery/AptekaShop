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
    public class ZakLeksController : Controller
    {
        private readonly ApplicationDbContext2 _context;

        public ZakLeksController(ApplicationDbContext2 context)
        {
            _context = context;
        }

        // GET: ZakLeks
        public async Task<IActionResult> Index()
        {
            return View(await _context.ZakLeks.ToListAsync());
        }

        // GET: ZakLeks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zakLek = await _context.ZakLeks
                .FirstOrDefaultAsync(m => m.IDZL == id);
            if (zakLek == null)
            {
                return NotFound();
            }

            return View(zakLek);
        }

        // GET: ZakLeks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ZakLeks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDZL")] ZakLek zakLek)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zakLek);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zakLek);
        }

        // GET: ZakLeks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zakLek = await _context.ZakLeks.FindAsync(id);
            if (zakLek == null)
            {
                return NotFound();
            }
            return View(zakLek);
        }

        // POST: ZakLeks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDZL")] ZakLek zakLek)
        {
            if (id != zakLek.IDZL)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zakLek);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZakLekExists(zakLek.IDZL))
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
            return View(zakLek);
        }

        // GET: ZakLeks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zakLek = await _context.ZakLeks
                .FirstOrDefaultAsync(m => m.IDZL == id);
            if (zakLek == null)
            {
                return NotFound();
            }

            return View(zakLek);
        }

        // POST: ZakLeks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zakLek = await _context.ZakLeks.FindAsync(id);
            if (zakLek != null)
            {
                _context.ZakLeks.Remove(zakLek);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZakLekExists(int id)
        {
            return _context.ZakLeks.Any(e => e.IDZL == id);
        }
    }
}
