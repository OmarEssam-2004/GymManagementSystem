
using AutoMapper;
using GymManagementSystem.BLL.Coomon;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Members;
using GymManagementSystem.DAL;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
namespace GymManagement.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MemberService(

            IUnitOfWork unitOfWork,
            IMapper mapper

            )

        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public void CreateMember(CreateMemberViewModel model, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct)
        {

            var emailExists = await _unitOfWork.GetRepository<Member>().AnyAsync(M => M.Email == model.Email, ct);
            var phoneExists = await _unitOfWork.GetRepository<Member>().AnyAsync(M => M.Phone == model.Phone, ct);

            if (emailExists || phoneExists) return Result.ValidationFailed("Email or Phone Already Exists");

           var member = _mapper.Map<Member>(model);

             _unitOfWork.GetRepository<Member>().Add(member);
            var count = await _unitOfWork.SaveChangesAsync(ct);
            return count > 0 ? Result.Ok() : Result.Conflict("Failed to Create Member");
        }

        public async Task<Result> DeleteMemberAsync(int memberId, CancellationToken ct)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if (member is null) return Result.NotFound("Member Not Found");

           var hasFutureSessions = await _unitOfWork.GetRepository<Booking>().AnyAsync(B => B.MemberId == memberId && B.Session.StartDate > DateTime.Now, ct);
            if (hasFutureSessions) return Result.Conflict("Member Cannot Be Deleted");

            _unitOfWork.GetRepository<Member>().Delete(member);
            var count = await _unitOfWork.SaveChangesAsync(ct);
            return count > 0 ? Result.Ok() : Result.Conflict("Failed to Delete Member");
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);

            var result = _mapper.Map<IEnumerable<MemberViewModel>>(members);
            return result;
        }

        public string? GetMemberById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct)
        {
          var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if(member is null) return null;

           var model = _mapper.Map<MemberViewModel>(member);

           var activeMembership = await _unitOfWork.GetRepository<MemberShip>().FirstOrDefaultAsync(M => M.MemberId == memberId && M.EndDate > DateTime.Now, ct);

            if(activeMembership is not null)
            {
                model.Phone = activeMembership.Plan.Name;
                model.MembershipStartDate = activeMembership.CreatedAt.ToString();
                model.MembershipEndDate = activeMembership.EndDate.ToString();
            }

            return model;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct)
        {
           var healthRecord = await _unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(H => H.MemberId == memberId, ct);

            if(healthRecord is null) return null;

           var model = _mapper.Map<HealthRecordViewModel>(healthRecord);

            return model;
                
        }
        
        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct)
        {
           var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);

           var model = _mapper.Map<MemberToUpdateViewModel>(member);

            return model;
        }

        public async Task<Result> UpdateMemberAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if (member is null) return Result.NotFound("Member Not Found");


            var emailExists = await _unitOfWork.GetRepository<Member>().AnyAsync(M => M.Email == model.Email && M.Id != memberId, ct);
            var phoneExists = await _unitOfWork.GetRepository<Member>().AnyAsync(M => M.Phone == model.Phone && M.Id != memberId, ct);

            if (emailExists || phoneExists) return Result.ValidationFailed("Email or Phone Already Exists");

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.Address.City = model.City;
            member.Address.Street = model.Street;

            _unitOfWork.GetRepository<Member>().Update(member);
            var count = await _unitOfWork.SaveChangesAsync(ct);
            return count > 0 ? Result.Ok() : Result.Conflict("Failed to Update Member");

        }

    }
}





