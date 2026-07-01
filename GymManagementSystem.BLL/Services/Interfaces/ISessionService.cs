using GymManagementSystem.BLL.Coomon;
using GymManagementSystem.BLL.ViewModels.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionViewModel>> GetAllSessionAsync(CancellationToken ct);
        Task<Result> CreateSessionAsync(CreateSessionViewModel model,CancellationToken ct);
        Task<IEnumerable<TrainerSelectViewModel>> GetAllTrainersForDropDownAsync( CancellationToken ct = default);
        Task<IEnumerable<CategorySelectViewModel>> GetAllCategoriesForDropDownAsync( CancellationToken ct = default);
        Task<Result<SessionViewModel>> GetSessionDetailsByIdAsync(int SessionId, CancellationToken ct = default);
        Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int SessionId, CancellationToken ct = default);
        Task<Result> UpdateSessionAsync(int SessionId, UpdateSessionViewModel model, CancellationToken ct = default);
        Task<Result> DeleteSessionAsync(int SessionId, CancellationToken ct = default);

         
    }
}
