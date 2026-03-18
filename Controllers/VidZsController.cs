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
    public class VidZsController : Controller
    {
        private readonly ApplicationDbContext2 _context;

        public VidZsController(ApplicationDbContext2 context)
        {
            _context = context;
        }

        // GET: VidZs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vids.ToListAsync());
        }

        // GET: VidZs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vidZ = await _context.Vids
                .FirstOrDefaultAsync(m => m.IDV == id);
            if (vidZ == null)
            {
                return NotFound();
            }

            return View(vidZ);
        }

        // GET: VidZs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VidZs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDV,Naz")] VidZ vidZ)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vidZ);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vidZ);
        }

        // GET: VidZs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vidZ = await _context.Vids.FindAsync(id);
            if (vidZ == null)
            {
                return NotFound();
            }
            return View(vidZ);
        }

        // POST: VidZs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDV,Naz")] VidZ vidZ)
        {
            if (id != vidZ.IDV)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vidZ);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VidZExists(vidZ.IDV))
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
            return View(vidZ);
        }

        // GET: VidZs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vidZ = await _context.Vids
                .FirstOrDefaultAsync(m => m.IDV == id);
            if (vidZ == null)
            {
                return NotFound();
            }

            return View(vidZ);
        }

        // POST: VidZs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vidZ = await _context.Vids.FindAsync(id);
            if (vidZ != null)
            {
                _context.Vids.Remove(vidZ);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VidZExists(int id)
        {
            return _context.Vids.Any(e => e.IDV == id);
        }
    }
}
