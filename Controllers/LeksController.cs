using System.Net.Http.Headers;
using apteka.Data;
using apteka.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apteka.Controllers
{
    public class LeksController : Controller
    {
        private readonly ApplicationDbContext2 _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpClientFactory _httpClientFactory;

        public LeksController(
            ApplicationDbContext2 context,
            IWebHostEnvironment environment,
            IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _environment = environment;
            _httpClientFactory = httpClientFactory;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("IDL,Naz,Qena")] Lek lek,
            IFormFile? imageFile,
            string? imageUrl,
            string? imageSource)
        {
            if (imageSource == "upload")
            {
                if (imageFile == null || imageFile.Length == 0)
                {
                    ModelState.AddModelError("Foto", "Выберите файл изображения.");
                }
            }
            else if (imageSource == "url")
            {
                if (string.IsNullOrWhiteSpace(imageUrl))
                {
                    ModelState.AddModelError("Foto", "Укажите ссылку на изображение.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    lek.Foto = await SaveImageAsync(imageSource, imageFile, imageUrl);
                    _context.Add(lek);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("Foto", ex.Message);
                }
                catch (HttpRequestException)
                {
                    ModelState.AddModelError("Foto", "Не удалось загрузить изображение по ссылке.");
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

        private async Task<string?> SaveImageAsync(string? imageSource, IFormFile? imageFile, string? imageUrl)
        {
            if (imageSource == "upload")
            {
                return await SaveUploadedImageAsync(imageFile!);
            }

            if (imageSource == "url")
            {
                return await SaveImageFromUrlAsync(imageUrl!);
            }

            return null;
        }

        private async Task<string> SaveUploadedImageAsync(IFormFile imageFile)
        {
            if (imageFile.Length == 0)
            {
                throw new InvalidOperationException("Файл изображения пустой.");
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

        private async Task<string> SaveImageFromUrlAsync(string imageUrl)
        {
            if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out var uri))
            {
                throw new InvalidOperationException("Ссылка на изображение имеет неверный формат.");
            }

            var client = _httpClientFactory.CreateClient();
            using var response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            if (!IsImageContent(response.Content.Headers.ContentType))
            {
                throw new InvalidOperationException("Указанная ссылка не содержит изображение.");
            }

            var extension = GetExtensionFromUri(uri);
            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = GetExtensionFromContentType(response.Content.Headers.ContentType?.MediaType);
            }

            if (string.IsNullOrWhiteSpace(extension))
            {
                throw new InvalidOperationException("Не удалось определить формат изображения.");
            }

            var fileName = $"{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(GetImageDirectoryPath(), fileName);

            await using var sourceStream = await response.Content.ReadAsStreamAsync();
            await using var destinationStream = new FileStream(filePath, FileMode.Create);
            await sourceStream.CopyToAsync(destinationStream);

            return fileName;
        }

        private string GetImageDirectoryPath()
        {
            var imageDirectory = Path.Combine(_environment.WebRootPath, "Img");
            Directory.CreateDirectory(imageDirectory);
            return imageDirectory;
        }

        private static bool IsImageContent(MediaTypeHeaderValue? contentType)
        {
            return contentType?.MediaType?.StartsWith("image/", StringComparison.OrdinalIgnoreCase) == true;
        }

        private static string? GetExtensionFromUri(Uri uri)
        {
            var extension = Path.GetExtension(uri.AbsolutePath);
            return string.IsNullOrWhiteSpace(extension) ? null : extension;
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
