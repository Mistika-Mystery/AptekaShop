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
    public class ProizsController : Controller
    {
        private readonly ApplicationDbContext2 _context;

        public ProizsController(ApplicationDbContext2 context)
        {
            _context = context;
        }

        // GET: Proizs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Proizs.ToListAsync());
        }

        // GET: Proizs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proiz = await _context.Proizs
                .FirstOrDefaultAsync(m => m.IDPr == id);
            if (proiz == null)
            {
                return NotFound();
            }

            return View(proiz);
        }

        // GET: Proizs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proizs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDPr,Naz")] Proiz proiz)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proiz);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proiz);
        }

        // GET: Proizs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proiz = await _context.Proizs.FindAsync(id);
            if (proiz == null)
            {
                return NotFound();
            }
            return View(proiz);
        }

        // POST: Proizs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDPr,Naz")] Proiz proiz)
        {
            if (id != proiz.IDPr)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proiz);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProizExists(proiz.IDPr))
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
            return View(proiz);
        }

        // GET: Proizs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proiz = await _context.Proizs
                .FirstOrDefaultAsync(m => m.IDPr == id);
            if (proiz == null)
            {
                return NotFound();
            }

            return View(proiz);
        }

        // POST: Proizs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proiz = await _context.Proizs.FindAsync(id);
            if (proiz != null)
            {
                _context.Proizs.Remove(proiz);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProizExists(int id)
        {
            return _context.Proizs.Any(e => e.IDPr == id);
        }
    }
}
