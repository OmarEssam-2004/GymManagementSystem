using GymManagementSystem.BLL.Coomon;
using GymManagementSystem.BLL.ViewModels.Members;
using GymManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct);
        Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct);
        Task<Result> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct);
        Task<Result> UpdateMemberAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct);
        Task<Result> DeleteMemberAsync(int memberId, CancellationToken ct);
        Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct);
        Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct);
 
    }
}
