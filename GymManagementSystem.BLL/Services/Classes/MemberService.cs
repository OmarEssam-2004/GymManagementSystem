
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Members;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
namespace GymManagement.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _memberRepository;

        public MemberService(IGenericRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public void CreateMember(CreateMemberViewModel model, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct)
        {

            var emailExists = await _memberRepository.AnyAsync(M => M.Email == model.Email, ct);
            var phoneExists = await _memberRepository.AnyAsync(M => M.Phone == model.Phone, ct);

            if (emailExists || phoneExists) return false;


            var member = new Member
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Gender = model.Gender,
                Address = new Address
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street
                },
                HealthRecord = new HealthRecord
                {
                    Height = model.HealthRecordViewModel.Height,
                    Weight = model.HealthRecordViewModel.Weight,
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Note = model.HealthRecordViewModel.Note,
                }

            };

            var count = await _memberRepository.AddAsync(member, ct);
            return count > 0;
        }

        public Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct)
        {
            var members = await _memberRepository.GetAllAsync(ct: ct);

            var membersViewModel = members.Select(member => new MemberViewModel()
            {
                Id = member.Id,
                Name = member.Name,
                Photo = member.Photo,
                Phone = member.Phone,
                Email = member.Email,
                Gender = member.Gender.ToString()
            });


            return membersViewModel;
        }

        public Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<MemberToUpdateViewModel> GetMemberToUpdateAsync(int memberId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateMemberAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}













//-------------------------------------------------------------------------------------------


//using GymManagementSystem.BLL.Services.Interfaces;
//using GymManagementSystem.BLL.ViewModels.Members;
//using GymManagementSystem.DAL.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GymManagementSystem.BLL.Services.Classes
//{
//    public class MemberService : IMemberService
//    {
//        private readonly TGenericRepository<Member> _memberRepository;
//        public MemberService(TGenericRepository<Member> memberRepository)
//        {
//            _memberRepository = memberRepository;
//        }

//        public Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct)
//        {
//            var members = await _memberRepository.GetAllAsync(ct :ct);

//            var membersViewModels = members.Select(member => new MemberViewModel
//            {
//                Id = member.Id,
//                Name = member.Name,
//                Photo = member.Photo,
//                Phone = member.Phone,
//                Email = member.Email,
//                Gender = member.Gender.ToString()
//            }).ToList();

//            return membersViewModels;
//        }

//        public Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<bool> UpdateMemberAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}


//using GymManagementSystem.BLL.Services.Interfaces;
//using GymManagementSystem.DAL.Models;
//using GymManagementSystem.DAL.Repositories.Interfaces;
//using GymManagementSystem.BLL.Services.Interfaces;
//using GymManagementSystem.BLL.ViewModels.Members;
//using GymManagementSystem.BLL.ViewModels.MemberViewModels;
//using GymManagementSystem.DAL.Models;
//using GymManagementSystem.DAL.Repositories.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;



