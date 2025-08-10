using HotelBooking.Data;
using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;
using HotelBooking.Services.AdminPanelServices;
using HotelBooking.Services.ApiModule;
using HotelBooking.Services.Contracts;
using HotelBooking.Services.Contracts.AdminPanelContracts;
using HotelBooking.Services.Contracts.HotelsServicesContracts;
using HotelBooking.Web.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Services.StarsService;
using Newtonsoft.Json;
using NuGet.Protocol;
using HotelBooking.Services.HotelsServices;

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


//Api Configuration Services
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<IStarsService, StarsService>();

//Hotels Services
builder.Services.AddScoped<IGetHotelsService, GetHotelsService>();
builder.Services.AddScoped<IGetBookedHotelsService, GetBookedHotelsService>();

//Identity Services
builder.Services.AddScoped<UserRole>();

//Admin Panel Services
builder.Services.AddScoped<IGetCheckoutedHotelsService, GetCheckoutedHotelsService>();
builder.Services.AddScoped<IAdminPanelDeleteBookingService, AdminPanelDeleteBookingsService>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
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
