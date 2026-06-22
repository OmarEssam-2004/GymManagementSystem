using GymManagementSystem.DAL.Repositories.Classes;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.DbContexts;
using GymManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymManagementSystem.Controllers
{
    public class PlansController : Controller
    {
        //private readonly GymDbConext _context = new GymDbConext();
        private readonly IGenericRepository<Plan> _PlanRepository;
        public PlansController(IGenericRepository<Plan> planRepository)
        {
            _PlanRepository = planRepository;
        }


        //Indext        
        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            //var plans = await _context.PlanS.ToListAsync();
            var plans = await _PlanRepository.GetAllAsync(ct:ct);
            return View(plans);
        }

        //Detalis
        public async Task<IActionResult> Details(int id, CancellationToken ct = default)
        {
            //var plans = await _context.PlanS.FirstOrDefaultAsync(P => P.Id == id);
            var plans = await _PlanRepository.GetByIdAsync(id,ct);
            if (plans is null) return RedirectToAction(nameof(Index));

            return View(plans);
        }

         



    }
}
