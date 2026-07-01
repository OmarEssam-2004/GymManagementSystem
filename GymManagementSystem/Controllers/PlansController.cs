
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Plans;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagementSystem.Controllers
{
    public class PlansController : Controller
    {
        private readonly IPlanService _planService;

        public PlansController(IPlanService planService)
        {
            _planService = planService;
        }

        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var plans = await _planService.GetAllPlansAsync(ct);
            return View(plans);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct = default)
        {
            var plan = await _planService.GetPlanDetailsAsync(id, ct);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan not found!";
                return RedirectToAction("Index");
            }
            return View(plan);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct = default)
        {
            var plan = await _planService.GetPlanToUpdateAsync(id, ct);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan not found!";
                return RedirectToAction("Index");
            }
            return View(plan);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PlanToUpdateViewModel model, CancellationToken ct = default)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _planService.UpdatePlanAsync(id, model, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Plan updated successfully!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = result.error;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id, CancellationToken ct = default)
        {
            var result = await _planService.TogglePlanStatusAsync(id, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Plan Status Changed";
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
            }
            return RedirectToAction("Index");
        }
    }
}