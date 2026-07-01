using GymManagementSystem.BLL.Coomon;
using GymManagementSystem.BLL.ViewModels.Trainers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface ITrainerService
    {
        Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct);
        Task<TrainerViewModel?> GetTrainerDetailsAsync(int trainerId, CancellationToken ct);
        Task<Result> CreateTrainerAsync(TrainerToAddViewModel model, CancellationToken ct);
        Task<Result> UpdateTrainerAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct);
        Task<Result> DeleteTrainerAsync(int trainerId, CancellationToken ct);
        Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct);
    }
}