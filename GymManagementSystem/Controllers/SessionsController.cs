using GymManagement.BLL.Services.Classes;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Sessions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GymManagementSystem.PL.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct = default)
        {

           var result = await _sessionService.GetAllSessionAsync(ct);

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct = default)
        {
           ViewBag.Trainers = new SelectList(await _sessionService.GetAllTrainersForDropDownAsync(ct), "Id","Name");
           ViewBag.Category = new SelectList(await _sessionService.GetAllCategoriesForDropDownAsync(ct), "Id", "CategoryName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model,CancellationToken ct = default)
        {
            if (ModelState.IsValid)
            {
                var result = await _sessionService.CreateSessionAsync(model, ct);

                if (result)
                {
                    TempData["SuccessMessage"] = "Session created successfully :)";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to create Session :(";
                }

                return RedirectToAction("Index");
            }
            return View(model);
        }


    }
}
