using HotelBooking.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.CodeAnalysis.CSharp.Syntax;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    //.AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.AddControllersWithViews();


builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddRoleManager<RoleManager<IdentityRole>>()
        .AddDefaultUI()
        .AddDefaultTokenProviders();

//
var serviceProvider = builder.Services.BuildServiceProvider();
var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

if (!await roleManager.RoleExistsAsync("Admin"))
{
    await roleManager.CreateAsync(new IdentityRole("Admin"));
}

if (!await roleManager.RoleExistsAsync("User"))
{
    await roleManager.CreateAsync(new IdentityRole("User"));
}

//var adminEmail = "admin123@gmail.com";
//var admin = await userManager.FindByEmailAsync(adminEmail);

//if (admin == null)
//{
//    admin = new IdentityUser
//    {
//        UserName = adminEmail,
//        Email = adminEmail
//    };
//    await userManager.CreateAsync(admin, "AdminPassword@123");
//    await userManager.AddToRoleAsync(admin, "Admin");
//}


var app = builder.Build();



app.UseExceptionHandler("/Home/StatusCodeError?errorCode={0}");
app.UseStatusCodePagesWithRedirects("/Home/StatusCodeError?errorCode={0}");

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


//
app.Use(async (context, next) =>
{
    var userManager = context.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = context.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();

    if (!await roleManager.RoleExistsAsync("admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("admin"));
    }

    if (!await roleManager.RoleExistsAsync("user"))
    {
        await roleManager.CreateAsync(new IdentityRole("user"));
    }

    await next();
});
//



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "account",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapRazorPages();

app.Run();
