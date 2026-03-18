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
    public class LeksController : Controller
    {
        private readonly ApplicationDbContext2 _context;

        public LeksController(ApplicationDbContext2 context)
        {
            _context = context;
        }

        // GET: Leks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Leks.ToListAsync());
        }

        // GET: Leks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lek = await _context.Leks
                .FirstOrDefaultAsync(m => m.IDL == id);
            if (lek == null)
            {
                return NotFound();
            }

            return View(lek);
        }

        // GET: Leks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Leks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDL,Naz,Qena,Foto")] Lek lek)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lek);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lek);
        }

        // GET: Leks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lek = await _context.Leks.FindAsync(id);
            if (lek == null)
            {
                return NotFound();
            }
            return View(lek);
        }

        // POST: Leks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDL,Naz,Qena,Foto")] Lek lek)
        {
            if (id != lek.IDL)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lek);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LekExists(lek.IDL))
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
            return View(lek);
        }

        // GET: Leks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lek = await _context.Leks
                .FirstOrDefaultAsync(m => m.IDL == id);
            if (lek == null)
            {
                return NotFound();
            }

            return View(lek);
        }

        // POST: Leks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lek = await _context.Leks.FindAsync(id);
            if (lek != null)
            {
                _context.Leks.Remove(lek);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LekExists(int id)
        {
            return _context.Leks.Any(e => e.IDL == id);
        }
    }
}
