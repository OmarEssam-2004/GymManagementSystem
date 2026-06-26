using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Plans;
using GymManagementSystem.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GymManagementSystem.DAL.Models; 
using GymManagementSystem.DAL.Repositories.Interfaces; 

namespace GymManagement.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IGenericRepository<Plan> _planRepository;
        private readonly IGenericRepository<MemberShip> _memberShipRepository;

        public PlanService(
            IGenericRepository<Plan> planRepository,
            IGenericRepository<MemberShip> memberShipRepository)
        {
            _planRepository = planRepository;
            _memberShipRepository = memberShipRepository;
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct)
        {
            var plans = await _planRepository.GetAllAsync(ct: ct);
            return plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                DurationDays = p.DurationDays,
                Price = p.Price,
                Description = p.Description,
                IsActive = p.IsActive
            });
        }

        public async Task<PlanViewModel?> GetPlanDetailsAsync(int planId, CancellationToken ct)
        {
            var plan = await _planRepository.GetByIdAsync(planId, ct);
            if (plan is null) return null;

            return new PlanViewModel
            {
                Id = plan.Id,
                Name = plan.Name,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                Description = plan.Description,
                IsActive = plan.IsActive
            };
        }

        public async Task<PlanToUpdateViewModel?> GetPlanToUpdateAsync(int planId, CancellationToken ct)
        {
            var plan = await _planRepository.GetByIdAsync(planId, ct);
            if (plan is null) return null;

            return new PlanToUpdateViewModel
            {
                Id = plan.Id,
                Name = plan.Name,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                Description = plan.Description
            };
        }

        public async Task<bool> UpdatePlanAsync(int planId, PlanToUpdateViewModel model, CancellationToken ct)
        {
            var plan = await _planRepository.GetByIdAsync(planId, ct);
            if (plan is null) return false;
            if (plan.Name != model.Name) return false;

            var hasActiveMemberships = await _memberShipRepository.AnyAsync(m => m.PlanId == planId && m.EndDate > DateTime.Now, ct);
            if (hasActiveMemberships) return false;

            plan.DurationDays = model.DurationDays;
            plan.Price = model.Price;
            plan.Description = model.Description;

            var count = await _planRepository.UpdateAsync(plan, ct);
            return count > 0;
        }

        public async Task<bool> TogglePlanStatusAsync(int planId, CancellationToken ct)
        {
            var plan = await _planRepository.GetByIdAsync(planId, ct);
            if (plan is null) return false;

            if (plan.IsActive)
            {
                var hasActiveMemberships = await _memberShipRepository.AnyAsync(m => m.PlanId == planId && m.EndDate > DateTime.Now, ct);
                if (hasActiveMemberships) return false;
            }

            plan.IsActive = !plan.IsActive;
            var count = await _planRepository.UpdateAsync(plan, ct);
            return count > 0;
        }
    }
}
