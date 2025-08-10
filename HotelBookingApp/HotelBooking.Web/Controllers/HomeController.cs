using HotelBooking.Services.ApiModule;
using HotelBooking.Services.StarsService;
using HotelBooking.Services.ViewModels;
using HotelBooking.Services.Contracts;
using HotelBooking.Web.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Diagnostics;
using HotelBooking.Data;
using HotelBooking.Models.AppModels;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Privacy()
        {
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        public IActionResult ErrorWithStatusCode(int errorCode)
        {
            ViewBag.StatusCode = errorCode;
            return View();
        }
    }
}