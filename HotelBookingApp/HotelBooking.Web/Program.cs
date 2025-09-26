using HotelBooking.Data;
using HotelBooking.Models.Identity;
using HotelBooking.Services.AdminPanelServices;
using HotelBooking.Services.Contracts.AdminPanelContracts;
using HotelBooking.Services.Contracts.HotelsServicesContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Services.HotelsServices;
using Microsoft.Extensions.AI;
using HotelBooking.Services.AIServices;
using HotelBooking.Services.BookingApiConfiguration;
using HotelBooking.Services.Contracts.AIServicesContracts;
using HotelBooking.Services.Contracts.BookingApiConfigurationContracts;
using Microsoft.Extensions.Options;
using DotNetEnv;
using HotelBooking.Data.SeedWork;
using HotelBooking.Services.Contracts.AdminPanelServicesContracts;
using HotelBooking.Services.Contracts.EmailServicesContracts;
using HotelBooking.Services.Contracts.KafkaOperationsLoggerPublisherContracts;
using HotelBooking.Services.EmailServices;
using HotelBooking.Services.KafkaOperationsLoggerPublisher;
using HotelBooking.Web.Hubs;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// Identity/DB services
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BookingDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<UserModel>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 2;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredUniqueChars = 0;

    }).AddRoles<UserRole>()
      .AddEntityFrameworkStores<BookingDbContext>()
      .AddSignInManager<SignInManager<UserModel>>();

builder.Services.AddScoped<UserRole>();

//Api Configuration Services
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services
    .AddOptions<RapidApiOptions>()
    .Bind(builder.Configuration.GetSection(RapidApiOptions.SectionName))
    .ValidateDataAnnotations()
    .Validate(o => !string.IsNullOrWhiteSpace(o.Key), "RapidApi:Key is required.")
    .ValidateOnStart();

builder.Services.AddHttpClient("RapidApiBooking", (sp, client) =>
{
    var opts = sp.GetRequiredService<IOptions<RapidApiOptions>>().Value;
    client.BaseAddress = new Uri((opts.BaseUrl ?? "").TrimEnd('/') + "/");
    client.DefaultRequestHeaders.Add("x-rapidapi-host", opts.Host);
    client.DefaultRequestHeaders.Add("x-rapidapi-key", opts.Key);
});

builder.Services.AddTransient<HttpClient>(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("RapidApiBooking"));

//Hotels Services
builder.Services.AddScoped<IGetHotelsService, GetHotelsService>();
builder.Services.AddScoped<IGetBookingsService, GetBookingsService>();
builder.Services.AddScoped<IAddToCartService, AddToCartService>();
builder.Services.AddScoped<IRemoveBookingService, RemoveBookingService>();
builder.Services.AddScoped<ICheckoutBookingsService, CheckoutBookingsService>();

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//EmailSender
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddTransient<IEmailSender, MailKitEmailSender>();
builder.Services.AddSingleton<IEmailTemplatePathProviderService, EmailTemplatePathProviderService>();
builder.Services.AddSingleton<IGetEmailTemplateFromPathService, GetEmailTemplateFromPathService>();
builder.Services.AddSingleton<IGetCheckoutedBookingsEmailTemplateHtmlWithParametersService, GetCheckoutedBookingsEmailTemplateHtmlWithParametersService>();
builder.Services.AddScoped<ICheckoutEmailService, CheckoutEmailService>();
builder.Services
    .AddSingleton<IGetRegisteredAccountEmailTemplateHtmlWithParametersService,
        GetRegisteredAccountEmailTemplateHtmlWithParametersService>();
builder.Services.AddScoped<IRegisterAccountEmailService, RegisterAccountEmailService>();

//Admin Panel Services
builder.Services.AddScoped<IGetCheckoutedHotelsService, GetCheckoutedHotelsService>();
builder.Services.AddScoped<IAdminPanelDeleteBookingService, AdminPanelDeleteBookingsService>();

//AI Services
builder.Services.AddChatClient(new OllamaChatClient(new Uri("http://localhost:11434"), "qwen2.5:7b-instruct"));
builder.Services.AddScoped<IAskAppAiService, AskAppAiService>();

//SignalR
builder.Services.AddSignalR();

builder.Services.AddSingleton<IOperationsLogger, KafkaOperationsLogger>();


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

app.MapHub<AdminNotificationsHub>("/adminNotificationsHub");

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

public partial class Program { }
