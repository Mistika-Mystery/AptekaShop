[HttpPost]
public async Task<IActionResult> UploadImage(IFormFile file)
{
    if (file == null || file.Length == 0)
        return BadRequest("Файл не выбран.");

    // Путь к wwwroot/webp
    string webpFolder = Path.Combine(_env.WebRootPath, "webp");

    // Создание папки, если её нет
    if (!Directory.Exists(webpFolder))
        Directory.CreateDirectory(webpFolder);

    // Уникальное имя файла
    string uniqueFileName = Guid.NewGuid().ToString() + ".webp";
    string filePath = Path.Combine(webpFolder, uniqueFileName);

    // Сохранение файла
    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    // Возврат относительного URL
    string fileUrl = $"/webp/{uniqueFileName}";
    return Ok(new { FileUrl = fileUrl });
}