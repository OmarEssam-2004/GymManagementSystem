using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Trainers;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagementSystem.PL.Controllers
{
    public class TrainersController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainersController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var trainers = await _trainerService.GetAllTrainersAsync(ct);
            return View(trainers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TrainerToAddViewModel model, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                var result = await _trainerService.CreateTrainerAsync(model, ct);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Trainer created successfully !";
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
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var result = await _trainerService.GetTrainerDetailsAsync(id, ct);
            if (result is null)
            {
                TempData["ErrorMessage"] = "Trainer not found !";
                return RedirectToAction("Index");
            }

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var result = await _trainerService.GetTrainerToUpdateAsync(id, ct);
            if (result is null)
            {
                TempData["ErrorMessage"] = "Edit Trainer not found !!";
                return RedirectToAction("Index");
            }

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> EditTrainer(int id, TrainerToUpdateViewModel model, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                var result = await _trainerService.UpdateTrainerAsync(id, model, ct);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Trainer Update successfully !";
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
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _trainerService.GetTrainerDetailsAsync(id, ct);
            if (result is null)
            {
                TempData["ErrorMessage"] = "Delete Trainer not found !!";
                return RedirectToAction("Index");
            }
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _trainerService.DeleteTrainerAsync(id, ct);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Trainer Deleted successfully !";
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
            }

            return RedirectToAction("Index");
        }
    }
}