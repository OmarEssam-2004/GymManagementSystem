using GymManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct);
        Task<int>GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct = default);
        Task<Session> GetSessionByIdWithTrainerAndCategoryAsync(int sessionId, CancellationToken ct = default);
    }
}
