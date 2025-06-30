using CompanyTaskManager.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CompanyTaskManager.Web.Controllers
{
    public class HomeController(ILogger<HomeController> _logger) : Controller
    {
        public IActionResult Index()
        {
            var userName = User?.Identity?.Name ?? "Anonymous";
            _logger.LogInformation("User {UserName} accessed home index page", userName);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            var userName = User?.Identity?.Name ?? "Anonymous";
            
            _logger.LogWarning("Error page accessed by user {UserName} with RequestId {RequestId}", 
                userName, requestId);
                
            return View(new ErrorViewModel { RequestId = requestId });
        }
    }
}
