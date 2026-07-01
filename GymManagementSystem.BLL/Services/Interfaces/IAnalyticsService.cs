using GymManagementSystem.BLL.ViewModels.AnalyticsViewModels;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface IAnalyticsService
    {
        Task<AnalyticsViewModel> GetDashboardAnalyticsAsync(CancellationToken ct);
    }
}