using UrlShortener.Models;
using UrlShortener.Repositories;
using UrlShortener.Services;
using UrlShortener.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IEncodingService, EncodingService>();
var dbConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddTransient<IUrlRepository>(x => new UrlRepository(dbConnection));
builder.Services.AddTransient<IValidator<UrlViewModel>, UrlValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
