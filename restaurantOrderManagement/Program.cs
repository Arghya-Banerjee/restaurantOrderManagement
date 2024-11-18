using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
//builder.Services.AddControllersWithViews().AddJsonOptions(o => o.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString);

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(15);// Set session time out 15 minute
});
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();







app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Menu}/{action=Index}/{id?}");


app.MapControllers();

if (app.Environment.IsProduction())
{
    app.UseWebSockets();
}
app.UseSession();
app.Run();
