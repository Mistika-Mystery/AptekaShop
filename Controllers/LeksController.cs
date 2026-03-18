using apteka.Data;
using apteka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apteka.Controllers
{
    public class LeksController : Controller
    {
        private readonly ApplicationDbContext2 _context;
        private readonly IWebHostEnvironment _environment;

        public LeksController(
            ApplicationDbContext2 context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Leks
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Leks.ToListAsync());
        }

        // GET: Leks/Details/5
        [AllowAnonymous]
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("IDL,Naz,Qena")] Lek lek,
            IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("Foto", "Выберите файл изображения.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    lek.Foto = await SaveUploadedImageAsync(imageFile!);
                    _context.Add(lek);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("Foto", ex.Message);
                }
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("IDL,Naz,Qena,Foto")] Lek lek,
            IFormFile? imageFile)
        {
            if (id != lek.IDL)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingLek = await _context.Leks
                        .AsNoTracking()
                        .FirstOrDefaultAsync(item => item.IDL == id);

                    if (existingLek == null)
                    {
                        return NotFound();
                    }

                    if (imageFile is { Length: > 0 })
                    {
                        lek.Foto = await SaveUploadedImageAsync(imageFile);
                        DeleteImageIfExists(existingLek.Foto);
                    }
                    else
                    {
                        lek.Foto = existingLek.Foto;
                    }

                    _context.Update(lek);
                    await _context.SaveChangesAsync();
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("Foto", ex.Message);
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

        private async Task<string> SaveUploadedImageAsync(IFormFile imageFile)
        {
            if (imageFile.Length == 0)
            {
                throw new InvalidOperationException("Файл изображения пустой.");
            }

            if (!IsImageContent(imageFile.ContentType))
            {
                throw new InvalidOperationException("Можно загружать только файлы изображений.");
            }

            var extension = Path.GetExtension(imageFile.FileName);
            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = GetExtensionFromContentType(imageFile.ContentType);
            }

            if (string.IsNullOrWhiteSpace(extension))
            {
                throw new InvalidOperationException("Не удалось определить формат изображения.");
            }

            var fileName = $"{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(GetImageDirectoryPath(), fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return fileName;
        }

        private string GetImageDirectoryPath()
        {
            var imageDirectory = Path.Combine(_environment.WebRootPath, "Img");
            Directory.CreateDirectory(imageDirectory);
            return imageDirectory;
        }

        private void DeleteImageIfExists(string? fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return;
            }

            var filePath = Path.Combine(GetImageDirectoryPath(), fileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        private static bool IsImageContent(string? contentType)
        {
            return contentType?.StartsWith("image/", StringComparison.OrdinalIgnoreCase) == true;
        }

        private static string? GetExtensionFromContentType(string? contentType)
        {
            return contentType?.ToLowerInvariant() switch
            {
                "image/jpeg" or "image/jpg" => ".jpg",
                "image/png" => ".png",
                "image/gif" => ".gif",
                "image/webp" => ".webp",
                "image/bmp" => ".bmp",
                "image/svg+xml" => ".svg",
                _ => null
            };
        }
    }
}
