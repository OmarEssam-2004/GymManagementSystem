using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.AnalyticsViewModels;
using GymManagementSystem.DAL;
using GymManagementSystem.DAL.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AnalyticsViewModel> GetDashboardAnalyticsAsync(CancellationToken ct)
        {
            var now = DateTime.Now;

            var totalMembers = await _unitOfWork.GetRepository<Member>().CountAsync(ct);

            var activeMembers = await _unitOfWork.GetRepository<MemberShip>()
                .CountAsync(M => M.EndDate > now, ct);

            var totalTrainers = await _unitOfWork.GetRepository<Trainer>().CountAsync(ct);

            var upcomingSessions = await _unitOfWork.GetRepository<Session>()
                .CountAsync(S => S.StartDate > now, ct);

            var ongoingSessions = await _unitOfWork.GetRepository<Session>()
                .CountAsync(S => S.StartDate <= now && S.EndDate >= now, ct);

            var completedSessions = await _unitOfWork.GetRepository<Session>()
                .CountAsync(S => S.EndDate < now, ct);

            return new AnalyticsViewModel
            {
                TotalMembers = totalMembers,
                ActiveMembers = activeMembers,
                TotalTrainers = totalTrainers,
                UpcomingsMembers = upcomingSessions, 
                OngoingSessions = ongoingSessions,
                CompletedSessions = completedSessions
            };
        }
    }
}