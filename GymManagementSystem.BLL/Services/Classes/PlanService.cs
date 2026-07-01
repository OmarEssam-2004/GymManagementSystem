
using AutoMapper;
using GymManagementSystem.BLL.Coomon;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Plans;
using GymManagementSystem.DAL;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);
            var result = _mapper.Map<IEnumerable<PlanViewModel>>(plans);
            return result;
        }

        public async Task<PlanViewModel?> GetPlanDetailsAsync(int planId, CancellationToken ct)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);
            if (plan is null) return null;

            var model = _mapper.Map<PlanViewModel>(plan);
            return model;
        }

        public async Task<PlanToUpdateViewModel?> GetPlanToUpdateAsync(int planId, CancellationToken ct)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);
            if (plan is null) return null;

            var model = _mapper.Map<PlanToUpdateViewModel>(plan);
            return model;
        }

        public async Task<Result> UpdatePlanAsync(int planId, PlanToUpdateViewModel model, CancellationToken ct)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);
            if (plan is null) return Result.NotFound("Plan Not Found");
            if (plan.Name != model.Name) return Result.ValidationFailed("Plan Name Mismatch");

            var hasActiveMemberships = await _unitOfWork.GetRepository<MemberShip>().AnyAsync(m => m.PlanId == planId && m.EndDate > DateTime.Now, ct);
            if (hasActiveMemberships) return Result.Conflict("Plan Cannot Be Updated");

            plan.DurationDays = model.DurationDays;
            plan.Price = model.Price;
            plan.Description = model.Description;

            _unitOfWork.GetRepository<Plan>().Update(plan);
            var count = await _unitOfWork.SaveChangesAsync(ct);
            return count > 0 ? Result.Ok() : Result.Conflict("Failed to Update Plan");
        }

        public async Task<Result> TogglePlanStatusAsync(int planId, CancellationToken ct)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);
            if (plan is null) return Result.NotFound("Plan Not Found");

            if (plan.IsActive)
            {
                var hasActiveMemberships = await _unitOfWork.GetRepository<MemberShip>().AnyAsync(m => m.PlanId == planId && m.EndDate > DateTime.Now, ct);
                if (hasActiveMemberships) return Result.Conflict("Plan Cannot Be Deactivated");
            }

            plan.IsActive = !plan.IsActive;
            _unitOfWork.GetRepository<Plan>().Update(plan);
            var count = await _unitOfWork.SaveChangesAsync(ct);
            return count > 0 ? Result.Ok() : Result.Conflict("Failed to Toggle Plan Status");
        }
    }
}
