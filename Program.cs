using apteka.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Добавление контекста базы данных
builder.Services.AddDbContext<ApplicationDbContext2>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавление сервисов MVC
builder.Services.AddControllersWithViews();
// Добавление поддержки сессий
builder.Services.AddSession();

var app = builder.Build();

// Настройка HTTP-запросов
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // Значение HSTS по умолчанию — 30 дней. Вы можете изменить его для производственных сценариев.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Использование сессий
app.UseSession();

app.UseRouting();

// Настройка авторизации
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
