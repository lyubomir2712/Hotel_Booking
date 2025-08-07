using HotelBooking.Data;
using HotelBooking.Models.Identity;
using HotelBooking.Services.ApiModule;
using HotelBooking.Services.Contracts;
using HotelBooking.Web.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Services.StarsService;
using Newtonsoft.Json;
using NuGet.Protocol;
using HotelBooking.Services.HotelAddService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<BookingDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<UserModel>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 2;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredUniqueChars = 0;
    })

    .AddRoles<UserRole>()
    .AddEntityFrameworkStores<BookingDbContext>()
    .AddSignInManager<SignInManager<UserModel>>();

builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<IStarsService, StarsService>();
builder.Services.AddScoped<HotelAddService>();
builder.Services.AddScoped<UserRole>();


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();



//app.UseExceptionHandler("/Home/ErrorWithStatusCode?errorCode={0}");
//app.UseStatusCodePagesWithRedirects("/Home/ErrorWithStatusCode?errorCode={0}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "bookedHotelsByUser",
    pattern: "Home/BookedHotels",
    defaults: new { controller = "BookingsCart", action = "GetBookedHotels" });



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");




app.MapRazorPages();

app.Run();
