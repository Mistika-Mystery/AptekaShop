using apteka.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Äîáàâëåíèå êîíòåêñòà áàçû äàííûõ
builder.Services.AddDbContext<ApplicationDbContext2>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Äîáàâëåíèå ñåðâèñîâ MVC
builder.Services.AddControllersWithViews();
// Äîáàâëåíèå ïîääåðæêè ñåññèé
builder.Services.AddSession();

var app = builder.Build();

// Íàñòðîéêà HTTP-çàïðîñîâ
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // Çíà÷åíèå HSTS ïî óìîë÷àíèþ — 30 äíåé. Âû ìîæåòå èçìåíèòü åãî äëÿ ïðîèçâîäñòâåííûõ ñöåíàðèåâ.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Èñïîëüçîâàíèå ñåññèé
app.UseSession();

app.UseRouting();

// Íàñòðîéêà àâòîðèçàöèè
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
