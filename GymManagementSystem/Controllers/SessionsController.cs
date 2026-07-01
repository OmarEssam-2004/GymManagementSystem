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

                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Session created successfully :)";
                }
                else
                {
                    TempData["ErrorMessage"] = result.error;
                }

                return RedirectToAction("Index");
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int Id, CancellationToken ct = default)
        {
            var result = await _sessionService.GetSessionDetailsByIdAsync(Id, ct);
            if (result.Success) 
                return View(result.Value);
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id, CancellationToken ct = default)
        {
            var result = await _sessionService.GetSessionToUpdateAsync(Id, ct);
            if (result.Success)
            {
                ViewBag.Trainers = new SelectList(await _sessionService.GetAllTrainersForDropDownAsync(ct), "Id", "Name");
                return View(result.Value);
            }
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int Id, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            if (ModelState.IsValid)
            {
                var result = await _sessionService.UpdateSessionAsync(Id, model, ct);
                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Session updated successfully :)";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Trainers = new SelectList(await _sessionService.GetAllTrainersForDropDownAsync(ct), "Id", "Name");
                    TempData["ErrorMessage"] = result.error;
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id, CancellationToken ct = default)
        {
            var result = await _sessionService.GetSessionDetailsByIdAsync(Id, ct);
            if (result.Success)                    
                return View(result.Value);         
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int Id, CancellationToken ct = default)
        {

                var result = await _sessionService.DeleteSessionAsync(Id, ct);
                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Session Deleted successfully :)";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = result.error;
                    return RedirectToAction("Index");
                }         

        }

    }
}
