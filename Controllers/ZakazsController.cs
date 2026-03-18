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
    public class ZakazsController : Controller
    {
        private readonly ApplicationDbContext2 _context;

        public ZakazsController(ApplicationDbContext2 context)
        {
            _context = context;
        }

        // GET: Zakazs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Zakazs.ToListAsync());
        }

        // GET: Zakazs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zakaz = await _context.Zakazs
                .FirstOrDefaultAsync(m => m.IDZ == id);
            if (zakaz == null)
            {
                return NotFound();
            }

            return View(zakaz);
        }

        // GET: Zakazs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Zakazs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDZ,Data")] Zakaz zakaz)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zakaz);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zakaz);
        }

        // GET: Zakazs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zakaz = await _context.Zakazs.FindAsync(id);
            if (zakaz == null)
            {
                return NotFound();
            }
            return View(zakaz);
        }

        // POST: Zakazs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDZ,Data")] Zakaz zakaz)
        {
            if (id != zakaz.IDZ)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zakaz);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZakazExists(zakaz.IDZ))
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
            return View(zakaz);
        }

        // GET: Zakazs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zakaz = await _context.Zakazs
                .FirstOrDefaultAsync(m => m.IDZ == id);
            if (zakaz == null)
            {
                return NotFound();
            }

            return View(zakaz);
        }

        // POST: Zakazs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zakaz = await _context.Zakazs.FindAsync(id);
            if (zakaz != null)
            {
                _context.Zakazs.Remove(zakaz);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZakazExists(int id)
        {
            return _context.Zakazs.Any(e => e.IDZ == id);
        }
    }
}
