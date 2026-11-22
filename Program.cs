using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using proiect_dotnet.Data;
using proiect_dotnet.Models;

var builder = WebApplication.CreateBuilder(args);

// SQL Server database (connection string from appsettings.json)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");
// TODO (Pas 4): Ce face AddDbContext si de unde isi ia baza de date adresa (connection string)?
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// ASP.NET Core Identity with our user class and roles (Admin/User)
// TODO (Pas 4): Ce adauga .AddRoles<IdentityRole>()? Fara el, rolurile Admin/User nu functioneaza.
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequiredLength = 6;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // the Identity pages (Login/Register) are Razor Pages

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// TODO (Pas 4): De ce UseAuthentication vine INAINTE de UseAuthorization? (cine esti, apoi ce ai voie)
app.UseAuthentication(); // must come BEFORE UseAuthorization
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

// Create the roles (Admin/User) and the admin account at startup.
// TODO (Pas 7): De ce avem nevoie de CreateScope aici (UserManager/DbContext au viata scurta, per cerere)?
using (var scope = app.Services.CreateScope())
{
    await SeedData.InitializeAsync(scope.ServiceProvider);
}

app.Run();
