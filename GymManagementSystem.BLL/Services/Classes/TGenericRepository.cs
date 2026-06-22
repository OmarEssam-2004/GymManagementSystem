using GymManagementSystem.BLL.ViewModels.Members;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class TGenericRepository<T>
    {
        internal async Task<IEnumerable<MemberViewModel>> GetAllAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}