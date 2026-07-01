using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using GymManagementSystem.BLL.Services.Interfaces; 
using GymManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnalyticsService _analyticsService; 

        public HomeController(ILogger<HomeController> logger, IAnalyticsService analyticsService)
        {
            _logger = logger;
            _analyticsService = analyticsService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var analyticsData = await _analyticsService.GetDashboardAnalyticsAsync(ct);
            return View(analyticsData);
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
    }
}