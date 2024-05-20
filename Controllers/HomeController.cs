using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SessionDemo.Models;

namespace SessionDemo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var value = HttpContext.Session.GetString("PrivacyWasAccessed");
        if (string.IsNullOrEmpty(value)) 
        {
            value = "NO";
        }
        ViewBag.PrivacyWasAccessed = value;
        return View();
    }

    public IActionResult Privacy()
    {
        HttpContext.Session.SetString("PrivacyWasAccessed", "YES");

        var value = HttpContext.Session.GetString("PrivacyWasAccessed");
        ViewBag.PrivacyWasAccessed = value;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
