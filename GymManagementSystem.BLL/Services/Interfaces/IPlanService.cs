using GymManagementSystem.BLL.ViewModels.Plans;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct);
        Task<PlanViewModel?> GetPlanDetailsAsync(int planId, CancellationToken ct);
        Task<PlanToUpdateViewModel?> GetPlanToUpdateAsync(int planId, CancellationToken ct);
        Task<bool> UpdatePlanAsync(int planId, PlanToUpdateViewModel model, CancellationToken ct);
        Task<bool> TogglePlanStatusAsync(int planId, CancellationToken ct);
    }
}